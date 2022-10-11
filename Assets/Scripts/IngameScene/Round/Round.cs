using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;
using System;

public class Round : MonoBehaviour
{
    [Header("Base Round")]
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] protected string _explanation = "";
    [SerializeField] ROUNDTYPE _roundType;

    protected IngamePlayerController _playerController = null;

    private List<Vector3> _playerSpawnPos;

    public void CreateRound()
    {
        gameObject.SetActive(false);

        Sprite[] roundSprites = Resources.LoadAll<Sprite>("Sprites/Round/" + GlobalData.map.ToString().ToLower());
        SpriteRenderer[] allChildren = GetComponentsInChildren<SpriteRenderer>();

        Func<string, Sprite> getSprite = (name) =>
        {
            foreach (Sprite sprite in roundSprites)
                if (sprite.name == name)
                    return sprite;
            return null;
        };

        foreach (SpriteRenderer child in allChildren)
        {
            Sprite sprite = getSprite(child.sprite.name);
            if (sprite != null)
                child.sprite = sprite;
        }

        _playerController = IngameScene.Instance.PlayerController;
    }

    public virtual void LoadRound()
    {
        Debug.Log($"Round {GlobalData.roundIndex} : Load");

        gameObject.SetActive(true);

        Transform[] playerSpawn = _playerSpawn.GetComponentsInChildren<Transform>();
        _playerSpawnPos = new List<Vector3>();
        for (int i = 0; i < GlobalData.teamUserCount; ++i)
            _playerSpawnPos.Add(playerSpawn[i + 1].position);
    }

    public virtual void StartRound()
    {
        Debug.Log($"Round {GlobalData.roundIndex} : Start");

        PlayerInput._type = _roundType;
        SetPlayerJumpHeight(0);
        TestManager.Instance.ClearRoundNum = -1;
    }

    public virtual void SendClearRound()
    {
        Debug.Log($"Round {GlobalData.roundIndex} : Send Clear");

        IngameScene.Instance.PacketHandler.SendStartRound(true);
    }

    public void SendReStartRound()
    {
        Debug.Log($"Round {GlobalData.roundIndex} : Send ReStart");

        IngameScene.Instance.PacketHandler.SendRestartRound();
    }

    public virtual void SetPlayerJumpHeight(float height)
    {
        var actor = P2PInGameManager.Instance.ControlActor;
        actor.SetJumpHeight(height);
    }

    public string GetExplanation()
    {
        return _explanation;
    }

    public List<Vector3> GetPlayerSpawn()
    {
        return _playerSpawnPos;
    }
}