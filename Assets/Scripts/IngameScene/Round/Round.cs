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
            if (child.sprite == null)
                continue;

            Sprite sprite = getSprite(child.sprite.name);
            if (sprite != null)
                child.sprite = sprite;
        }
    }

    public virtual void StartRound()
    {
        Debug.Log($"Round : Start  {gameObject.name}");

        _playerController = IngameScene.Instance.PlayerController;
    }

    public virtual void ClearRound(GameObject player)
    {
        Debug.Log($"Round : Clear {gameObject.name}");

        IngameScene.Instance.ClearRound();
    }

    public virtual void ReStartRound()
    {
        Debug.Log("Round : ReStart  {gameObject.name}");

        _playerController.LoadRound();
    }

    public string GetExplanation()
    {
        return _explanation;
    }

    public Vector3 GetPlayerSpawn()
    {
        return _playerSpawn.position;
    }

    public ROUNDTYPE GetRoundType()
    {
        return _roundType;
    }
}