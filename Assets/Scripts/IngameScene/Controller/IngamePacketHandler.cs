using CommonProtocol;
using EnumDef;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class IngamePacketHandler : MonoBehaviour
{
    private IngameScene _ingameScene;

    private void Start()
    {
        _ingameScene = IngameScene.Instance;
    }

    #region Send

    public void SendEnterGame()
    {
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

        var req = new IngameProcotol();
        IngameProcotol res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "StartGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvStartRound(res);
        }, !clearRound));
    }

    public void SendRestartRound()
    {
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
            _ingameScene.EnterGame();
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
            _ingameScene.LoadRound();
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
            _ingameScene.StartRound();
        }
        else
            Debug.LogAssertion($"ResReStartGame.ResponseType != Success");
    }

    public void RecvLastRound(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.isWinner = res.isWinner;
            _ingameScene.ClearGame();
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
            _ingameScene.DisConnectP2PServer();
        }
        else
            Debug.LogAssertion($"ResExitGame.ResponseType != Success");
    }

    #endregion


}