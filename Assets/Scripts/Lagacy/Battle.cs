using Assets.Scripts;
using BattleProtocol;
using BattleProtocol.Entities;
using CommonType;
using MessagePack;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using System;
using System.Net;
using UnityEngine;

public class Battle : MonoBehaviour
{
    private AsyncTcpSession _asyncTcpSession = new AsyncTcpSession();

    public void CreateGame()
    {
        var infos = ConfigReader.Instance.GetInfos<Infos>();

        var req = new CommonProtocol.ReqCreateGame
        {
            //MessageType = CommonProtocol.MessageType.CreateGame,
            UserId = GameManager.Instance.UserId,
            GameName = GameManager.Instance.Name
        };

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        var responseBytes
            = webClient.UploadString(new Uri("https://nq4m28r2tf.execute-api.ap-northeast-2.amazonaws.com/test/") + "CreateGame", "POST"
            , JsonConvert.SerializeObject(req));

        var res = JsonConvert.DeserializeObject<CommonProtocol.ResCreateGame>(responseBytes);

        //var webClient = new WebClient();
        //webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
        //var responseBytes
        //    = webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
        //    , MessagePackSerializer.Serialize(req));

        //var res = MessagePackSerializer.Deserialize<CommonProtocol.ResTryMatching>(responseBytes);

        if (res.ResponseType == CommonProtocol.ResponseType.Success)
        {
            var resPlayer = WebManager.Instance.CreatePlayer(res.GameSessionId);
            if (resPlayer.ResponseType == CommonProtocol.ResponseType.Success)
            {
                GameManager.Instance.PlayerType = PlayerType.Player1;
                BattleEnter(res.IpAddress, res.Port, res.GameSessionId, resPlayer.PlayerSessionId);
            }
        }
    }

    public void SearchRoom()
    {
        var infos = ConfigReader.Instance.GetInfos<Infos>();

        var req = new CommonProtocol.ReqUserId
        {
            //MessageType = CommonProtocol.MessageType.SearchGame,
            UserId = GameManager.Instance.UserId
        };

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        var responseBytes
            = webClient.UploadString(new Uri("https://nq4m28r2tf.execute-api.ap-northeast-2.amazonaws.com/test/") + "SearchGame", "POST"
            , JsonConvert.SerializeObject(req));

        var res = JsonConvert.DeserializeObject<CommonProtocol.ResSearchRoom>(responseBytes);

        //var webClient = new WebClient();
        //webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        //var responseBytes
        //    = webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
        //    , MessagePackSerializer.Serialize(req));

        //var res = MessagePackSerializer.Deserialize<CommonProtocol.ResSearchRoom>(responseBytes);

        GameManager.Instance.RoomListsCanvas.SetActive(true);
        GameManager.Instance.RoomListsCanvas.GetComponent<RoomsList>().SetValues(res);
    }


    public void TryMatching()
    {
        var infos = ConfigReader.Instance.GetInfos<Infos>();

        var req = new CommonProtocol.ReqTryMatching
        {
            MessageType = CommonProtocol.MessageType.TryMatching,
            UserId = GameManager.Instance.UserId
        };

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
        var responseBytes
            = webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
            , MessagePackSerializer.Serialize(req));

        var res = MessagePackSerializer.Deserialize<CommonProtocol.ResTryMatching>(responseBytes);

        if (res.ResponseType == CommonProtocol.ResponseType.Success)
        {
            //BattleEnter(res.IpAddress, res.Port);
        }
    }

    public void ForceBattleEnter()
    {
        //DELETE
        GameManager.Instance.PlayerType = PlayerType.Player1;
        BattleEnter("127.0.0.1", 50404, "gg", GameManager.Instance.PlayerType.ToString());
    }


    private void BattleEnter(string battleServerIp, int battleServerPot, string gameSessionId, string playerSessionId)
    {
        if (GameManager.Instance.IsTryMatching)
            return;

        GameManager.Instance.IsTryMatching = true;

        BattleServerConnector.Instance.Connect(battleServerIp, battleServerPot, "0");

        while (false == BattleServerConnector.Instance.IsConnected) ;

        GameManager.Instance.TryMatching();
        //GameManager.Instance.Knight.GetComponent<KnightControl>().running();
        GameManager.Instance.GameSessionId = gameSessionId;

        BattleServerConnector.Instance.Send(MessageType.BattleEnter,
                MessagePackSerializer.Serialize(new ProtoBattleEnter
                {
                    Msg = MessageType.BattleEnter,
                    UserId = GameManager.Instance.UserId,
                    GameSessionId = gameSessionId,
                    PlayerSessionId = playerSessionId,
                }));
    }

    public void BattleAttackRock()
    {
        BattleAttack(AttackType.Rock);
    }

    public void BattleAttackScissors()
    {
        BattleAttack(AttackType.Scissors);
    }

    public void BattleAttackPaper()
    {
        BattleAttack(AttackType.Paper);
    }

    public void BattleAttack(AttackType attackType)
    {
        Debug.Log(GameManager.Instance.PlayerType.ToString());
        Debug.Log(attackType.ToString());
        Debug.Log(GameManager.Instance.GameSessionId);
        
        BattleServerConnector.Instance.Send(MessageType.BattleAttack,
                MessagePackSerializer.Serialize(new ProtoBattleAttack
                {
                    Msg = MessageType.BattleAttack,
                    AttackType = attackType,
                    GameSessionId = GameManager.Instance.GameSessionId,
                    PlayerType = GameManager.Instance.PlayerType
                }));
    }

    public void BattleEnd()
    {
        //GameManager.Instance.Knight.GetComponent<KnightControl>().idle();
        GameManager.Instance.Knight.GetComponent<Knight>().ResetHealthBar();
        //GameManager.Instance.EnemyKnight.GetComponent<KnightControl>().idle();
        GameManager.Instance.EnemyKnight.GetComponent<Knight>().ResetHealthBar();

        GameManager.Instance.BattlePlayingCanvas.SetActive(false);
        GameManager.Instance.BattlePlayingCanvas.GetComponent<BattlePlaying>().BattleEnd();
        GameManager.Instance.BattleEndCanvas.SetActive(false);
        GameManager.Instance.EnemyKnight.SetActive(false);
        GameManager.Instance.LobbyCanvas.SetActive(true);
        GameManager.Instance.IsTryMatching = false;
    }
}
