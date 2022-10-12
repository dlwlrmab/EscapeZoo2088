using CommonProtocol;
using EnumDef;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class IngamePacketHandler : MonoBehaviour
{
    public static bool isTest = false;

    #region Send

    public void SendEnterGame()
    {
        if (isTest)
        {
            GlobalData.playerInfos = new List<PlayerInfo>() { new PlayerInfo() { animal = 1, Id = "1", MBTI = "isfj" } };
            GlobalData.myId = "1";
            IngameScene.Instance.EnterGame();
            return;
        }

        var req = new IngameProcotol();
        IngameProcotol res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "EnterIngame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvEnterGame(res);
        }));
    }

    public void SendStartRound(bool clearRound = false)
    {
        if (GlobalData.roundIndex + 1 == GlobalData.roundMax)
        {
            SendLastRound();
            return;
        }

        if (isTest)
        {
            GlobalData.roundIndex = GlobalData.roundIndex + 1;
            GlobalData.enemyRoundIndex = 1;
            GlobalData.sunriseTime = 3;
            IngameScene.Instance.LoadRound();
            return;
        }

        var req = new IngameProcotol();
        IngameProcotol res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "StartGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvStartRound(res);
        }, !clearRound));
    }

    private void StartRound()
    {
        IngameScene.Instance.StartRound();
    }

    public void SendRestartRound()
    {
        if (isTest)
        {
            Invoke("StartRound", 1);
            return;
        }

        var req = new IngameProcotol();
        IngameProcotol res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "RestartGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvRestartRound(res);
        }));
    }

    public void SendLastRound()
    {
        if (isTest)
        {
            GlobalData.isWinner = true;
            IngameScene.Instance.ClearGame();
            return;
        }

        var req = new IngameProcotol();
        IngameProcotol res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "LastRound", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvLastRound(res);
        }));
    }

    public void SendMatchResult()
    {
        var req = new IngameProcotol();
        IngameProcotol res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "MatchRequest", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvMatchResult(res);
        }));
    }

    public void SendExitGame()
    {
        var req = new IngameProcotol();

        req.teamUserCount = GlobalData.playingUserCount;

        IngameProcotol res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "ExitGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvExitGame(res);
        }));
    }

    #endregion

    #region Recv

    public void RecvEnterGame(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.playerInfos = res.playerInfos;
            GlobalData.playingUserCount = GlobalData.playerInfos.Count;
            IngameScene.Instance.EnterGame();
        }
        else
            Debug.LogAssertion($"ResEnterGame.ResponseType != Success");
    }

    public void RecvStartRound(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.roundIndex = res.currentRoundNum - 1;
            GlobalData.enemyRoundIndex = res.enemyRoundNum - 1;
            GlobalData.sunriseTime = res.sunriseTime;
            IngameScene.Instance.LoadRound();
        }
        else
            Debug.LogAssertion($"ResStartGame.ResponseType != Success");
    }

    public void RecvRestartRound(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.roundIndex = res.currentRoundNum - 1;
            GlobalData.enemyRoundIndex = res.enemyRoundNum - 1;
            GlobalData.sunriseTime = res.sunriseTime;
            IngameScene.Instance.StartRound();
        }
        else
            Debug.LogAssertion($"ResReStartGame.ResponseType != Success");
    }

    public void RecvLastRound(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.isWinner = res.isWinner;
            IngameScene.Instance.ClearGame();
            SendMatchResult();
        }
        else
            Debug.LogAssertion($"ResLastRound.ResponseType != Success");
    }

    public void RecvMatchResult(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.myScore = res.score;
        }
        else
            Debug.LogAssertion($"ResMatchResult.ResponseType != Success");
    }

    public void RecvExitGame(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            Debug.LogWarning("Lambda Server Disconnect");
            IngameScene.Instance.DisConnectP2PServer();
        }
        else
            Debug.LogAssertion($"ResExitGame.ResponseType != Success");
    }

    #endregion
}