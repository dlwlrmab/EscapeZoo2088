using System.Collections.Generic;
using UnityEngine;
using EnumDef;


public class IngamePacketHandler : MonoBehaviour
{
    #region Send

    public void SendInitGameComplete()
    {
        // 맵, 라운드들, 플레이어 등 생성 완료

        RecvStartRound();
    }

    public void SendRestartRound()
    {
        RecvRestartRound();
    }

    public void SendClearRound()
    {
        // 하나의 라운드 클리어 후 보냄

        if (GlobalData.roundIndex == GlobalData.roundMax)
            RecvClearGame();
        else
            RecvStartRound();
    }

    #endregion

    #region Recv

    public void RecvStartRound()
    {
        // 라운드 시작할 때 보냄

        GlobalData.roundIndex++;
        IngameScene.Instance.LoadRound();
    }

    public void RecvRestartRound()
    {
        // 라운드 재시작할 때 보냄

        IngameScene.Instance.StartRound();
    }

    public void RecvUpdateRound()
    {
        // 라운드 진행 중 업데이트할 데이터가 있을 때
        // 라운드 0: 태양 보여주기

        IngameScene.Instance.MapController.GetCurrentRound().UpdateRound();
    }

    public void RecvClearEnemyRound()
    {
        // 적이 라운드 클리어했을 때 보냄

        GlobalData.enemyRoundIndex++;
        IngameScene.Instance.ClearEnemyRound();
    }

    public void RecvClearGame()
    {
        // 모든 라운드 클리어 했을 때 보냄

        IngameScene.Instance.ClearGame(true);
    }

    #endregion

    #region Test

    private void Awake()
    {
        // 원래 로비에서 인게임 넘어올때 받아야 하지만,
        // 임시로 여기서 설정

        GlobalData.map = MAP.SNOW;
        GlobalData.roundList = new int[3] { 0, 1, 2 };
        GlobalData.playerInfos = new List<PlayerInfo>() {
            new PlayerInfo { Id = "121", Animal = ANIMAL.CHICKEN, MBTI = "isfj" },
            new PlayerInfo { Id = "123", Animal = ANIMAL.COW, MBTI = "isfj" },
            new PlayerInfo { Id = "124", Animal = ANIMAL.CROCODILE, MBTI = "isfj" },
            new PlayerInfo { Id = "125", Animal = ANIMAL.GORILLA, MBTI = "isfj" },
            new PlayerInfo { Id = "126", Animal = ANIMAL.PANDA, MBTI = "isfj" }
        };
        GlobalData.roundIndex = -1;
        GlobalData.roundMax = GlobalData.roundList.Length;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RecvStartRound();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RecvClearGame();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            RecvUpdateRound();
        }
    }

    #endregion
}