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
        // 라운드 시작(재시작)할 때 보냄

        GlobalData.roundIndex++;
        IngameScene.Instance.LoadRound();
    }

    public void RecvClearGame()
    {
        // 라운드 종료와 함께 받음

        IngameScene.Instance.ClearGame(true);
    }

    #endregion

    #region Test

    private void Awake()
    {
        // 원래 로비에서 인게임 넘어올때 받아야 하지만,
        // 임시로 여기서 설정

        GlobalData.map = MAP.SNOW;
        GlobalData.roundList = new int[6] { 0, 1, 2, 3, 4, 5 };
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
    }

    #endregion
}