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

    public void SendInitGameComplete()
    {
        var req = new ReqStartGame
        {
            preRoundNum = ++GlobalData.roundIndex,
            endRoundNum = GlobalData.roundMax,
            teamUserCount = GlobalData.teamUserCount,
        };

        ResStartGame res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "StartGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResStartGame>(responseString);
            RecvStartRound(res);
        }));
    }

    public void SendRestartRound()
    {
        var req = new ReqReStartGame
        {
            currentRoundNum = GlobalData.roundIndex,
            endRoundNum = GlobalData.roundMax,
            teamUserCount = GlobalData.teamUserCount,
        };

        ResReStartGame res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "RestartGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResReStartGame>(responseString);
            RecvRestartRound(res);
        }));
    }

    public void SendClearRound()
    {
        var req = new ReqMatchResult
        {
            isWinner = GlobalData.IsWinner,
        };

        ResMatchResult res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "MatchRequest", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResMatchResult>(responseString);
            RecvClearGame(res);
        }));
    }

    #endregion

    #region Recv

    public void RecvStartRound(ResStartGame res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.roundIndex = res.currentRoundNum;
            GlobalData.SunriseTime = res.sunriseTime;
            GlobalData.roundList = res.roundList;
            _ingameScene.LoadRound();
        }
        else
            Debug.LogAssertion($"ResStartGame.ResponseType != Success");
    }

    public void RecvRestartRound(ResReStartGame res)
    {
        if (res.ResponseType != ResponseType.Success)
        {
            GlobalData.roundIndex = res.currentRoundNum;
            GlobalData.roundList = res.roundList;
            GlobalData.SunriseTime = res.sunriseTime;
            _ingameScene.StartRound();
        }
        else
            Debug.LogAssertion($"ResReStartGame.ResponseType != Success");

    }

    public void RecvClearGame(ResMatchResult res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.IsWinner = true;
            GlobalData.myScore = res.score;
            _ingameScene.ClearGame();
        }
        else
            Debug.LogAssertion($"ResMatchResult.ResponseType != Success");
    }

    #endregion
}