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
    [SerializeField] Transform _appearObj;

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

    protected void ResetAppearObj()
    {
        if(_appearObj != null)
        {
            for(int i = 0; i < _appearObj.childCount; i++)
            {
                _appearObj.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public virtual void StartRound()
    {
        Debug.Log($"Round : Start  {gameObject.name}");

        ResetAppearObj();
        _playerController = IngameScene.Instance.PlayerController;
    }

    public virtual void ClearRound(GameObject player)
    {
        Debug.Log($"Round : Clear {gameObject.name}");

        IngameScene.Instance.ClearRound();
    }

    public virtual void ReStartRound()
    {
        Debug.Log($"Round : ReStart  {gameObject.name}");

        ResetAppearObj();
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