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
    }

    public virtual void StartRound()
    {
        Debug.Log($"Round {GlobalData.roundIndex} : Start");

        PlayerInput._type = _roundType;
        SetPlayerJumpHeight(0);
    }

    public int clearPlayerCount = 0;

    public virtual void SendClearRound()
    {
        Debug.Log($"Round {GlobalData.roundIndex} : Send Clear");

        clearPlayerCount++;
        if (clearPlayerCount == 2)
            IngameScene.Instance.PacketHandler.SendStartRound();
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

    public Vector3 GetPlayerSpawn()
    {
        return _playerSpawn.position;
    }
}