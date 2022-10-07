using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class IngamePlayerController : MonoBehaviour
{
    private List<Player> _playerList;

    private bool _isCreateComplete = false;
    public bool CreateComplete { get { return _isCreateComplete; } }

    public void CreatePlayer(List<PlayerInfo> playerInfos)
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
}
