using Assets.Scripts.Connectors;
using BattleProtocol;
using SuperSocket.ClientEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using Object = System.Object;

public sealed class BattleServerConnector : Singleton<BattleServerConnector>
{
    private readonly AsyncTcpSession _asyncTcpSession = new AsyncTcpSession();
    private readonly Queue<DataEventCopy> _dataEventArgs = new Queue<DataEventCopy>();
    private readonly int _intSize = sizeof(int);
    private readonly Object _lock = new Object();
    private readonly Object _sendLock = new Object();
    //private readonly NetSyncTime _syncTime = new NetSyncTime();

    private bool _isConnect;
    private Coroutine _coroutine;

    public string RoomId { get; internal set; }
    public string Ip { get; private set; }
    public bool IsConnected => _asyncTcpSession.IsConnected;

    private void OnApplicationQuit()
    {
        Close();
    }

    private IEnumerator CoUpdate()
    {
        while (_isConnect)
        {
            //_syncTime.Update();
            yield return null;

            DataEventCopy dataEventCopy;
            lock (_lock)
            {
                if (_dataEventArgs.Count == 0)
                    continue;

                dataEventCopy = _dataEventArgs.Dequeue();
            }
            ProcessDataEventArgs(dataEventCopy);
        }

        _coroutine = null;
        //if (GameManager.Instance.CurrentScene == SceneType.MainBattleScene && BattleManager.Instance.IsEndBattle == false)
        //    BattleManager.Instance.OpenReconnectPopup();
    }

    public void Connect(string ip, int port, string roomId)
    {
        //BattleInfo.BattleState = BattleState.BattleServerConnectWait;
        RoomId = roomId;
        Ip = ip;
        _isConnect = true;
        //_syncTime.Init();

        StartUpdateCoroutine();

        if (_asyncTcpSession.IsConnected)
            return;

        _asyncTcpSession.Connected += AsyncTcpSessionConnected;
        _asyncTcpSession.DataReceived += AsyncTcpSessionDataReceived;
        _asyncTcpSession.Closed += AsyncTcpSessionClosed;

        try
        {
            if (IPAddress.TryParse(ip, out var ipAddress) == false)
            {
                var hostEntry = Dns.GetHostEntry(ip);
                ipAddress = hostEntry.AddressList[0];
            }
            _asyncTcpSession.Connect(new IPEndPoint(ipAddress, port));
        }
        catch (Exception e)
        {
        }
    }

    public void Close()
    {
        try
        {
            //ASDebug.Log("BattleServer Try Close");
            _isConnect = false;
            _asyncTcpSession.Close();
            _asyncTcpSession.Connected -= AsyncTcpSessionConnected;
            _asyncTcpSession.DataReceived -= AsyncTcpSessionDataReceived;
            _asyncTcpSession.Closed -= AsyncTcpSessionClosed;

            lock (_lock)
            {
                _dataEventArgs.Clear();
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    public void Send(MessageType messageType, byte[] data)
    {
        lock (_sendLock)
        {
            if (_asyncTcpSession.IsConnected == false)
            {
                //BattleManager.Instance.OpenReconnectPopup();
                return;
            }

#if DEBUG_ENABLE
            if (messageType != MessageType.SyncTime)
                ASDebug.Log("[BattleServer Send] <<", messageType, ">> IsConnected:", _asyncTcpSession.IsConnected);
#endif
            var header = BitConverter.GetBytes((int)messageType);
            using (var stream = new MemoryStream(header.Length + _intSize + data.Length))
            {
                stream.Write(header, 0, header.Length);
                stream.Write(BitConverter.GetBytes(data.Length), 0, _intSize);
                stream.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                _asyncTcpSession.Send(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
    }

    private void AsyncTcpSessionDataReceived(object sender, DataEventArgs e)
    {
        lock (_lock)
        {
            _dataEventArgs.Enqueue(new DataEventCopy(e.Data, e.Length));
        }
    }

    private void ProcessDataEventArgs(DataEventCopy e)
    {
        int offset = 0;
        var sizeHeader = sizeof(MessageType);
        while (offset < e.Length)
        {
            var byteArrayHeader = e.Data.CloneRange(offset, sizeHeader);
            var byteArrayBodyLength = e.Data.CloneRange(offset + sizeHeader, _intSize);
            var bodyLength = BitConverter.ToInt32(byteArrayBodyLength, 0);
            var body = e.Data.CloneRange(offset + sizeHeader + _intSize, bodyLength);

            offset += sizeHeader + _intSize + bodyLength;

            var msgType = (MessageType)BitConverter.ToInt32(byteArrayHeader, 0);
            var process = ProtocolProcessGenerator.GetProcess(msgType);
            var proto = ProtocolFactory.DeserializeProtocol(msgType, body);
            process?.OnResponse(proto);
        }
    }

    private void AsyncTcpSessionConnected(object sender, EventArgs e)
    {
        //BattleInfo.BattleState = BattleState.BattleServerConnectDone;
        //_syncTime.AsyncTcpSessionConnected();
        //ASDebug.Log("[Connector] 배틀 서버 접속 성공");
    }

    private void AsyncTcpSessionClosed(object sender, EventArgs e)
    {
        //ASDebug.Log("[Connector] 배틀 서버 접속 해제");
        //_syncTime.AsyncTcpSessionClosed();
        _isConnect = false;
    }

    private void StartUpdateCoroutine()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(CoUpdate());
    }
}

public struct DataEventCopy
{
    public byte[] Data;
    public int Length;

    public DataEventCopy(byte[] data, int length)
    {
        Data = (byte[])data.Clone();
        Length = length;
    }
}