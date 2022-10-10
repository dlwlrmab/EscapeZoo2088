using CommonProtocol;
using EnumDef;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class IngamePacketHandler : MonoBehaviour
{
    #region Send

    public void SendEnterGame()
    {
        var req = new IngameProcotol();
        IngameProcotol res = null;

        RecvEnterGame(res);
    }

    public void SendStartRound()
    {
        if (GlobalData.roundIndex + 1 == GlobalData.roundMax)
        {
            SendLastRound();
            return;
        }

        var req = new IngameProcotol();
        IngameProcotol res = null;

        RecvStartRound(res);
    }

    public void SendRestartRound()
    {
        var req = new IngameProcotol();
        IngameProcotol res = null;

        RecvRestartRound(res);
    }

    public void SendLastRound()
    {
        var req = new IngameProcotol();
        IngameProcotol res = null;

        RecvLastRound(res);
    }

    #endregion

    #region Recv

    public void RecvEnterGame(IngameProcotol res)
    {
        GlobalData.roundList = new List<int>() { 2, 4, 5, 1, 0, 3 };
        GlobalData.roundIndex = -1;

        GlobalData.playerInfos = new List<PlayerInfo>() { 
            new PlayerInfo {  Id = "1", animal = 2, MBTI="ISFJ"},
            new PlayerInfo { Id = "2", animal = 5, MBTI = "ISTJ" }, 
            new PlayerInfo { Id = "3", animal = 7, MBTI = "ENFP" },
            new PlayerInfo { Id = "4", animal = 9, MBTI = "INFJ" }, 
            new PlayerInfo { Id = "5", animal = 11, MBTI = "INFJ" } };

        IngameScene.Instance.EnterGame();
    }

    public void RecvStartRound(IngameProcotol res)
    {
        GlobalData.roundIndex += 1;
        GlobalData.enemyRoundIndex += Mathf.Min(Random.Range(0,1), GlobalData.roundMax);
        GlobalData.sunriseTime = 5;
        IngameScene.Instance.LoadRound();
    }

    public void RecvRestartRound(IngameProcotol res)
    {
        GlobalData.enemyRoundIndex += Mathf.Min(Random.Range(0, 1), GlobalData.roundMax);
        IngameScene.Instance.StartRound();
    }

    public void RecvLastRound(IngameProcotol res)
    {
        GlobalData.isWinner = GlobalData.roundIndex >= GlobalData.enemyRoundIndex;
        IngameScene.Instance.ClearGame();
    }

    #endregion
}