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
        var req = new ReqEnterGame
        {
        };

        ResEnterGame res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "EnterGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResEnterGame>(responseString);
            RecvEnterGame(res);
        }));
    }

    public void SendStartRound()
    {
        if(GlobalData.roundIndex == GlobalData.roundMax)
        {
            SendLastRound();
            return;
        }

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

    public void SendLastRound()
    {
        var req = new ReqLastRound
        {
        };

        ResLastRound res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "LastRound", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResLastRound>(responseString);
            RecvLastRound(res);
        }));
    }

    public void SendMatchResult()
    {
        var req = new ReqMatchResult
        {
            isWinner = GlobalData.isWinner,
        };

        ResMatchResult res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "MatchRequest", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResMatchResult>(responseString);
            RecvMatchResult(res);
        }));
    }

    #endregion

    #region Recv

    public void RecvEnterGame(ResEnterGame res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.playerInfos = res.playerInfos;
            _ingameScene.EnterGame();
        }
        else
            Debug.LogAssertion($"ResEnterGame.ResponseType != Success");
    }

    public void RecvStartRound(ResStartGame res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.roundIndex = res.currentRoundNum;
            GlobalData.enemyRoundIndex = res.enemyRoundIndex;
            GlobalData.sunriseTime = res.sunriseTime;
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
            GlobalData.sunriseTime = res.sunriseTime;
            _ingameScene.StartRound();
        }
        else
            Debug.LogAssertion($"ResReStartGame.ResponseType != Success");
    }

    public void RecvLastRound(ResLastRound res)
    {
        if (res.ResponseType == ResponseType.Success)
        {
            GlobalData.isWinner = true;
            _ingameScene.ClearGame();
            SendMatchResult();
        }
        else
            Debug.LogAssertion($"ResLastRound.ResponseType != Success");
    }

    public void RecvMatchResult(ResMatchResult res)
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