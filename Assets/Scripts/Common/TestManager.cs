using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 테스트를 위한 키입력 등 처리
public class TestManager : Singleton<TestManager>
{
    public int ClearRoundNum { get; set; } = -1; // 한라운드당 한번만 클리어 입력받음, 라운드 시작 or 재시작시 초기화(-1)

    public void Init()
    {
        ClearRoundNum = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // p키 누르는경우 클리어
        if (Input.GetKeyDown(KeyCode.P) )
        {  
            if(ClearRoundNum != GlobalData.roundIndex && IngameScene.Instance.State == EnumDef.INGAME_STATE.PLAYING)
            {
                IngameScene.Instance.PacketHandler.SendStartRound(true);
                ClearRoundNum = GlobalData.roundIndex;
            }
        }

        // e키 누르는 경우 로비로 이동
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IngameScene.Instance.State == EnumDef.INGAME_STATE.PLAYING)
            {
                IngameScene.Instance.DisConnectP2PServer();
            }
        }
    }
}
