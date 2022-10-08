using MessagePack;
using System;
using System.Net;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SendProtocolManager : Singleton<SendProtocolManager>
{
    WebClient webClient = null;
    string responseString;
    byte[] responseBytes;

    bool _loadingMark = false;

    protected override void Awake()
    {
        base.Awake();
        webClient = new WebClient();
    }

    #region Lambda

    public IEnumerator CoSendLambdaReq(string str, string type, Action<string> a, bool loadingMark = true)
    {
        _loadingMark = loadingMark;
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

        Debug.Log($"send json {type} : {str}");

        if (_loadingMark)
            SceneLoadManager.Instance.SetLoading(true);

        yield return StartCoroutine(CoWaitLambdaRes(str,type));

        a?.Invoke(responseString);

        if(_loadingMark)
            SceneLoadManager.Instance.SetLoading(false);

        yield return null;
    }

    IEnumerator CoWaitLambdaRes(string str, string type)
    {
        responseString = webClient.UploadString(new Uri(GlobalData.GatewayAPI) + type, "POST", str);
        Debug.Log($"res json {type} : {responseString}");

        yield return null;
    }

    #endregion

    #region Protocol

    public IEnumerator CoSendProtocolReq(byte[] str, string type, Action<byte[]> a, bool loadingMark = true)
    {
        _loadingMark = loadingMark;
        webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";

        var message = MessagePackSerializer.Serialize(str);

        Debug.Log($"send message {type} : {message}");

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
        Debug.Log($"res message {type} : {responseBytes}");

        yield return null;
    }

    #endregion
}
