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

        _isCreateComplete = true;
    }

    public void LoadRound()
    {
        PlayerResetPreStep();
        Vector3 startPos = IngameScene.Instance.MapController.GetPlayerSpawn();
        for (int i = 0; i < _playerList.Count; ++i)
        {
            _playerList[i].LoadRound(startPos, transform);
            startPos.x += 2f;
        }
        Invoke("PlayerResetPostStep", 0.15f);
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
        _playerList.Sort();

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

    void PlayerResetPreStep()
    {
        foreach(var p in _playerList)
        {
            p.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    void PlayerResetPostStep()
    {
        foreach (var p in _playerList)
        {
            p.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
