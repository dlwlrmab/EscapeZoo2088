using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class IngamePlayerController : MonoBehaviour
{
    private List<Player> _playerList;

    private bool _isCreateComplete = false;
    public bool CreateComplete { get { return _isCreateComplete; } }

    public void CreatePlayer()
    {
        _playerList = new List<Player>();

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
            _playerList[i].SetRround(startPos);
            startPos.x += 1;
        }
    }

    public void StartRound()
    {
        // 플레이어 움직이기 시작
    }

    public List<Player> GetPlayerList()
    {
        return _playerList;
    }

    public void SetPlayerData(ROUNDTYPE type)
    {
        foreach (var player in _playerList)
        {
            var playerMove = player.GetComponent<PlayerMove>();
            if (playerMove != null)
                playerMove.Init(type);
        }
    }
}
