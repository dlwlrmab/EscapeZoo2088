using MessagePack;
using System;
using System.Net;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SendProtocolManager : Singleton<SendProtocolManager>
{
    WebClient webClient = null;
    string responseString;
    byte[] responseBytes;

    bool _loadingMark = false;
    Action<string> _callback = null;

    public bool _sendProtocol = false;

    protected override void Awake()
    {
        base.Awake();
        webClient = new WebClient();
    }

    #region Lambda

    public IEnumerator CoSendLambdaReq(string str, string type, Action<string> a,bool loadingMark = true, bool useCoroutne = false)
    {
        if (_sendProtocol)
            yield break;

        _loadingMark = loadingMark;
        _sendProtocol = true;
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

        Debug.Log($"[Send] json {type} : {str}");

        if (_loadingMark)
            SceneLoadManager.Instance.SetLoading(true);

        _callback = a;

        if (!useCoroutne)
        {
            SendLambdaReqAsync(str, type);
        }
        else
        {
            yield return StartCoroutine(CoWaitLambdaRes(str, type));

            a?.Invoke(responseString);

            if (_loadingMark)
                SceneLoadManager.Instance.SetLoading(false);
        }

        yield return null;
    }

    async void SendLambdaReqAsync(string str, string type)
    {
        try
        {
            responseString = await webClient.UploadStringTaskAsync(new Uri(GlobalData.GatewayAPI + type), "POST", str);
        }
        catch(WebException e)
        {
        }
        finally
        {

            Debug.Log($"[Res] json {type} : {responseString}");
            _callback?.Invoke(responseString);
        }

        if (_loadingMark)
            SceneLoadManager.Instance.SetLoading(false);

        _sendProtocol = false;
    }

    // 연속으로 프로토콜을 많이 전송하는경우 이전응답을 받기전 동일한 프토토콜을 전송해 오류가 발생하는경우가있어,
    // 코루틴을 사용하여 전송
    IEnumerator CoWaitLambdaRes(string str, string type)
    {
        responseString = webClient.UploadString(new Uri(GlobalData.GatewayAPI) + type, "POST", str);
        //webClient.UploadStringAsync(new Uri(GlobalData.GatewayAPI + type), "POST", str);
        //webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(UploadStringCallback2);
        Debug.Log($"[Res] json {type} : {responseString}");
        _sendProtocol = false;

        yield return null;

    }

    #endregion

    #region Protocol

    public IEnumerator CoSendProtocolReq(byte[] str, string type, Action<byte[]> a, bool loadingMark = true)
    {
        _loadingMark = loadingMark;
        webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";

        var message = MessagePackSerializer.Serialize(str);

        Debug.Log($"[Send] message {type} : {message}");

        yield return StartCoroutine(CoWaitProtocolaRes(str, type, loadingMark));

        a?.Invoke(responseBytes);
        if (_loadingMark)
            SceneLoadManager.Instance.SetLoading(false);
        yield return null;
    }

    IEnumerator CoWaitProtocolaRes(byte[] str, string type, bool loadingMark)
    {
        if (_loadingMark)
            SceneLoadManager.Instance.SetLoading(true);

        responseBytes = webClient.UploadData(GlobalData.GatewayAPI + type, "POST", str);
        Debug.Log($"[Res] message {type} : {responseBytes}");

        yield return null;
    }

    #endregion
}
