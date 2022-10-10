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
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "EnterGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<IngameProcotol>(responseString);
            RecvEnterGame(res);
        }));
    }

    public void SendStartRound()
    {
        if (GlobalData.roundIndex == GlobalData.roundMax)
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
        }));
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

    #endregion

    #region Recv

    public void RecvEnterGame(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.playerInfos = res.playerInfos;
            _ingameScene.EnterGame();
        }
        else
            Debug.LogAssertion($"ResEnterGame.ResponseType != Success");
    }

    public void RecvStartRound(IngameProcotol res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.roundIndex = res.currentRoundNum;
            GlobalData.enemyRoundIndex = res.enemyRoundNum;
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
            GlobalData.roundIndex = res.currentRoundNum;
            GlobalData.enemyRoundIndex = res.enemyRoundNum;
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

    #endregion
}