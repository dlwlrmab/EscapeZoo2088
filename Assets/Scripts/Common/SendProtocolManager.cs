using MessagePack;
using System;
using System.Net;
using UnityEngine;

public class SendProtocolManager : MonoBehaviour
{
    public static string SendLambdaReq(string str, string type)
    {
        var webClient = new WebClient();

        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

        Debug.Log($"send json {type} : {str}");

        var responseBytes = webClient.UploadString(new Uri(GlobalData.GatewayAPI) + type, "POST", str);

        Debug.Log($"res {type} : {responseBytes}");

        return responseBytes;
    }

    public static byte[] SendProtocolReq(byte[] str, string type)
    {
        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";

        var message = MessagePackSerializer.Serialize(str);

        Debug.Log($"send message {type} : {message}");

        var responseBytes = webClient.UploadData(GlobalData.GatewayAPI + type, "POST", message);

        Debug.Log($"res message {type} : {responseBytes}");

        return responseBytes;
    }
}
