using Assets.Scripts;
using CommonProtocol;
using Newtonsoft.Json;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class PopupLogin : MonoBehaviour
{
    public void OnLogin()
    {
        var name = GameManager.Instance.LoginCanvas.GetComponentInChildren<InputField>()
            .GetComponentInChildren<Text>().text;

        var req = new ReqAccountJoin
        {
            Name = name,
        };

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        var responseBytes
            = webClient.UploadString(new Uri("https://nq4m28r2tf.execute-api.ap-northeast-2.amazonaws.com/test/") + "Login", "POST"
            , JsonConvert.SerializeObject(req));

        //var webClient = new WebClient();
        //webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
        //var responseBytes
        //    = webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
        //    , MessagePackSerializer.Serialize(req));

        //var res = MessagePackSerializer.Deserialize<ResAccountJoin>(responseBytes);

        var res = JsonConvert.DeserializeObject<ResAccountJoin>(responseBytes);

        if (res.ResponseType == ResponseType.Fail)
        {
            GameManager.Instance.LoginCanvas.GetComponentInChildren<RawImage>()
            .GetComponentInChildren<Text>().text = "존재하 않는 이름입니다.";
        }
        else
        {
            GameManager.Instance.Name = name;
            GameManager.Instance.UserId = res.UserId;
            Success();
        }
    }

    public void OnAccountJoin()
    {
        var name = GameManager.Instance.LoginCanvas.GetComponentInChildren<InputField>()
            .GetComponentInChildren<Text>().text;

        if (string.IsNullOrWhiteSpace(name))
            return;

        var req = new ReqAccountJoin
        {
            Name = name
        };

        var webClient = new WebClient();

        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        var responseBytes
            = webClient.UploadString(new Uri("https://nq4m28r2tf.execute-api.ap-northeast-2.amazonaws.com/test/") + "AccountJoin", "POST"
            , JsonConvert.SerializeObject(req));

        var res = JsonConvert.DeserializeObject<ResAccountJoin>(responseBytes);

        //var webClient = new WebClient();
        //webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
        //var infos = ConfigReader.Instance.GetInfos<Infos>();
        //var responseBytes
        //    = webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
        //    , MessagePackSerializer.Serialize(req));

        //var res = MessagePackSerializer.Deserialize<ResAccountJoin>(responseBytes);

        if (res.ResponseType == ResponseType.DuplicateName)
        {
            GameManager.Instance.LoginCanvas.GetComponentInChildren<RawImage>()
            .GetComponentInChildren<Text>().text = "존재하는 이름입니다.";
        }
        else
        {
            GameManager.Instance.Name = name;
            GameManager.Instance.UserId = res.UserId;
            Success();
        }
    }

    private void Success()
    {
        GameManager.Instance.LoginCanvas.SetActive(false);
        GameManager.Instance.LobbyCanvas.SetActive(true);
        GameManager.Instance.LobbyCanvas.transform.Find("NameText").GetComponent<Text>().text = GameManager.Instance.Name;

        Destroy(GameManager.Instance.LoginCanvas);
    }
}
