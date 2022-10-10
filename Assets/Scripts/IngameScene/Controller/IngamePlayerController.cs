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
        P2PInGameManager.Instance.CreateMyPlayer();

    }

    public void LoadRound()
    {
        _playerList.Sort();
        Vector3 startPos = IngameScene.Instance.MapController.GetPlayerSpawn();
        for (int i = 0; i < _playerList.Count; ++i)
        {
            _playerList[i].LoadRound(startPos, transform);
            startPos.x += 0.5f;
        }
    }

    public void StartRound()
    {
        LoadRound();
    }

    public void ClearGame()
    {
        gameObject.SetActive(false);
    }

    public void AddPlayer(Player p)
    {
        _playerList.Add(p);

        if(_playerList.Count == 5)
            _isCreateComplete = true;

    }

    public void RemovePlayer(Player p)
    {
        if (_playerList.Contains(p))
            _playerList.Remove(p);
    }

    public List<Player> GetPlayerList()
    {
        return _playerList;
    }
}
