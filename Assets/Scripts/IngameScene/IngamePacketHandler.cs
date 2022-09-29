using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngamePacketHandler : MonoBehaviour
{
    #region Send

    public void SendGameLoadingComplete()
    {
        // 맵, 라운드들, 플레이어 등 생성 완료

        RecvRoundStart();
    }

    public void SendRoundClear()
    {
        // 하나의 라운드 클리어 후 보냄

        if (_testRound == 4)
            RecvGameResult();
        else
            RecvRoundStart();
    }

    #endregion

    #region Recv

    public void RecvEnterGame()
    {
        // 인게임 씬 입장한 후 보냄

        int[] roundList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        IngameScene.Instance.RecvEnterGame(roundList);
    }

    public void RecvRoundStart()
    {
        // 라운드 시작할 때 보냄

        int nextRound = _testRound++;
        IngameScene.Instance.RecvRoundStart(nextRound);
    }

    public void RecvGameResult()
    {
        // 라운드 종료와 함께 받음

        int rank = Random.Range(1, 5);
        IngameScene.Instance.RecvGameResult(rank);
    }

    #endregion

    #region Test

    int _testRound = 0;

    private void Awake()
    {
        RecvEnterGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) )
        {
            RecvRoundStart();
        }
        else if (Input.GetKeyDown(KeyCode.R) )
        {
            RecvGameResult();
        }
    }

    #endregion
}
