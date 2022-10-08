using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;
using EuNet.Unity;

public class IngamePlayerController : SceneSingleton<IngamePlayerController>
{
    private List<Player> _playerList = new List<Player>();

    private bool _isCreateComplete = false;
    public bool CreateComplete { get { return _isCreateComplete; } }

    public void CreatePlayer()
    {
        // to do
        // GlobalData.playerInfos 로 각 플레이어 동물 및 mbti 노출 작업 필요

        // 내 플레이어 생성
        //GameObject playerMe = Instantiate(Resources.Load<GameObject>("Player/PlayerMe"), transform);
        GameObject playerMe = P2PInGameManager.Instance.CreateMyPlayer();
        //_playerList.Add(playerMe.GetComponent<Player>()); ActorManager에서 관리

        // 다른 플레이어 생성: 입장시 자동 생성 ActorManager에서 관리
        _isCreateComplete = true;
    }

    public void LoadRound()
    {
        Vector3 startPos = IngameScene.Instance.MapController.GetPlayerSpawn();
        for (int i = 0; i < _playerList.Count; ++i)
        {
            _playerList[i].LoadRound(startPos, transform);
            startPos.x += 1;
        }
    }

    public void StartRound(ROUNDTYPE type)
    {
        for (int i = 0; i < _playerList.Count; ++i)
        {
            var playerMove = _playerList[i].GetComponent<PlayerMove>();
            if (playerMove != null)
                playerMove.Init(type);
        }
    }

    public List<Player> GetPlayerList()
    {
        return _playerList;
    }

    public void AddPlayer(Player p)
    {
        _playerList.Add(p);
    }

    public void RemovePlayer(Player p)
    {
        if(_playerList.Contains(p))
        {
            _playerList.Remove(p);
        }
    }
}
