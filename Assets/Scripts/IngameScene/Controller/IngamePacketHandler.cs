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
        SendStartRound(false);
    }

    public void SendStartRound(bool nextround = true)
    {
        var req = new ReqStartGame
        {
            endRoundNum = GlobalData.roundMax,
            teamUserCount = GlobalData.teamUserCount,
        };

        if (nextround)
            req.preRoundNum = ++GlobalData.roundIndex;
        else
            req.preRoundNum = GlobalData.roundIndex;


        ResStartGame res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "StartGame", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResStartGame>(responseString);
            if (res.ResponseType == ResponseType.Success)
            {
                GlobalData.roundIndex = res.currentRoundNum;
                GlobalData.SunriseTime = res.sunriseTime;
                GlobalData.roundList = res.roundList;
                RecvStartRound(res.currentRoundNum);
            }
            else
                Debug.LogAssertion($"ResStartGame.ResponseType != Success");
        }));
    }

    // 라운드 재시작할 때 보냄
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
        // 모든 라운드를 종료하였을때.
        if (GlobalData.roundIndex == GlobalData.roundMax - 1)
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

        // 하나의 라운드 클리어 후 보냄
        else
        {
            SendStartRound();
        }
    }

    #endregion

    #region Recv

    public void RecvStartRound(int round)
    {
        _ingameScene.LoadRound();
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

    public void RecvUpdateRound()
    {
        // 라운드 진행 중 업데이트할 데이터가 있을 때
        // 라운드 0: 태양 보여주기

        _ingameScene.MapController.GetCurrentRound().UpdateRound();
    }

    public void RecvClearEnemyRound()
    {
        // 적이 라운드 클리어했을 때 보냄

        GlobalData.enemyRoundIndex++;
        _ingameScene.ClearEnemyRound();
    }


    // 모든 라운드 종료 응답
    public void RecvClearGame(ResMatchResult res)
    {
        
        if(res.ResponseType == ResponseType.Success)
        {
            _ingameScene.ClearGame(res.score);
        }
        else
            Debug.LogAssertion($"ResMatchResult.ResponseType != Success");
    }

    #endregion

    #region Test

    private void Awake()
    {
        // 원래 로비에서 인게임 넘어올때 받아야 하지만,
        // 임시로 여기서 설정

        GlobalData.map = MAP.SNOW;
        GlobalData.roundList = new List<int>() { 0};
        GlobalData.playerInfos = new List<PlayerInfo>() {
            new PlayerInfo { Id = "121", Animal = ANIMAL.CHICKEN, MBTI = "isfj" },
            new PlayerInfo { Id = "123", Animal = ANIMAL.COW, MBTI = "isfj" },
            new PlayerInfo { Id = "124", Animal = ANIMAL.CROCODILE, MBTI = "isfj" },
            new PlayerInfo { Id = "125", Animal = ANIMAL.GORILLA, MBTI = "isfj" },
            new PlayerInfo { Id = "126", Animal = ANIMAL.PANDA, MBTI = "isfj" }
        };
        GlobalData.roundIndex = -1;
        GlobalData.roundMax = GlobalData.roundList.Count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //RecvStartRound();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //RecvClearGame();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            RecvUpdateRound();
        }
    }

    #endregion
}