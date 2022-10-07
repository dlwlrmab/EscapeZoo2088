using MessagePack;
using System;
using System.Net;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SendProtocolManager : Singleton<SendProtocolManager>
{
    WebClient webClient = null;
    
    protected override void Awake()
    {
        base.Awake();
        webClient = new WebClient();
    }

    public void SendLambdaReq(string str, string type, Action<string> action)
    {
        StartCoroutine(CoLambdaReq(str, type, action));
    }

    public void SendProtocolReq(byte[] str, string type, Action<byte[]> action)
    {
        StartCoroutine(CoProtocolReq(str, type, action));
    }

    public IEnumerator CoLambdaReq(string str, string type, Action<string> a)
    {
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

        Debug.Log($"send json {type} : {str}");

        string responseString = webClient.UploadString(new Uri(GlobalData.GatewayAPI) + type, "POST", str);

        Debug.Log($"res json {type} : {responseString}");

        a?.Invoke(responseString);
        yield return null;
    }

    IEnumerator CoProtocolReq(byte[] str, string type, Action<byte[]> a)
    {
        webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";

        var message = MessagePackSerializer.Serialize(str);

        Debug.Log($"send message {type} : {message}");

        byte[] responseBytes = webClient.UploadData(GlobalData.GatewayAPI + type, "POST", message);

        Debug.Log($"res message {type} : {responseBytes}");

        a?.Invoke(responseBytes);
        yield return null;
    }
}
