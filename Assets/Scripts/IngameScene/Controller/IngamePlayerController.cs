﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngamePlayerController : MonoBehaviour
{
    private List<Player> _playerList;

    private bool _isCreateComplete = false;
    public bool CreateComplete { get { return _isCreateComplete; } }

    public void CreatePlayer()
    {
        _playerList = new List<Player>();

        // 내 플레이어 생성
        GameObject playerMe = Instantiate(Resources.Load<GameObject>("Player/PlayerMe"), transform);
        _playerList.Add(playerMe.GetComponent<Player>());

        // 다른 플레이어 생성
        GameObject playerOther = Instantiate(Resources.Load<GameObject>("Player/PlayerOther"), transform);
        for (int i = 0; i < 4; ++i)
            _playerList.Add(playerOther.GetComponent<Player>());

        _isCreateComplete = true;
    }

    public void LoadRound()
    {
        Vector3 startPos = IngameScene.Instance.MapController.GetPlayerSpawn();

        for (int i = 0; i < _playerList.Count; ++i)
        {
            startPos.x += 5;
            _playerList[i].SetRround(startPos);
        }
    }

    public void StartRound()
    {
        // 플레이어 움직이기 시작
    }
}