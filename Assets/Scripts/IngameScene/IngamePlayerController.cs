using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngamePlayerController : MonoBehaviour
{
    private List<GameObject> _playerList;

    private bool _isCreateComplete = false;
    public bool CreateComplete { get { return _isCreateComplete; } }

    public void CreatePlayer()
    {
        _playerList = new List<GameObject>();
        for (int i = 0; i < 1; ++i) // 일단 한 명만 생성
            _playerList.Add(Instantiate(Resources.Load<GameObject>("Player/Player_" + i), transform));

        _isCreateComplete = true;
    }

    public void OnLoadRoundLoading()
    {
        // 맵 컨트롤러한테 플레이어 스폰 위치 받아오기
    }
}
