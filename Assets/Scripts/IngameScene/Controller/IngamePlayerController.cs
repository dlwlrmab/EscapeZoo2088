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
        GameObject playerMe = Instantiate(Resources.Load<GameObject>("Player/PlayerMe"), transform);
        _playerList.Add(playerMe.GetComponent<Player>());
        _playerList[0].CreatePlayer( playerInfos[0]);

        // 다른 플레이어 생성
        GameObject playerOther = Instantiate(Resources.Load<GameObject>("Player/PlayerOther"), transform);
        for (int i = 0; i < 4; ++i)
        {
            _playerList.Add(playerOther.GetComponent<Player>());
            _playerList[i + 1].CreatePlayer(playerInfos[i + 1]);
        }

        _isCreateComplete = true;
    }

    public void LoadRound()
    {
        Vector3 startPos = IngameScene.Instance.MapController.GetPlayerSpawn();
        for (int i = 0; i < _playerList.Count; ++i)
        {
            _playerList[i].LoadRound(startPos);
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
