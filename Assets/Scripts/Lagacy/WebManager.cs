using Assets.Scripts;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Net;

public class WebManager : Singleton<WebManager>
{
    public CommonProtocol.ResCreatePlayer CreatePlayer(string gameSessionId)
    {
        var infos = ConfigReader.Instance.GetInfos<Infos>();

        var req = new CommonProtocol.ReqCreatePlayer
        {
            //MessageType = CommonProtocol.MessageType.CreatePlayer,
            UserId = GameManager.Instance.UserId,
            GameSessionId = gameSessionId
        };

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        var responseBytes
            = webClient.UploadString(new Uri("https://nq4m28r2tf.execute-api.ap-northeast-2.amazonaws.com/test/") + "CreatePlayer", "POST"
            , JsonConvert.SerializeObject(req));

        return JsonConvert.DeserializeObject<CommonProtocol.ResCreatePlayer>(responseBytes);

        //var webClient = new WebClient();
        //webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
        //var responseBytes
        //    = webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
        //    , MessagePackSerializer.Serialize(req));

        //return MessagePackSerializer.Deserialize<CommonProtocol.ResCreatePlayer>(responseBytes);
    }
}
