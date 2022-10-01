using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class Round : MonoBehaviour
{
    [Header("Base Round")]
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] protected string _explanation = "";
    [SerializeField] ROUNDTYPE _roundType;

    protected IngamePlayerController _playerController = null;

    public void SetMap(int mapIndex)
    {
        SpriteRenderer[] allChildren = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in allChildren)
        {
            // 스프라이트 변경
        }
    }

    public virtual void StartRound()
    {
        Debug.Log("Round : StartRound");

        _playerController = IngameScene.Instance.PlayerController;
    }

    public virtual void ClearRound()
    {
        Debug.Log("Round : ClearRound");

        IngameScene.Instance.ClearRound();
    }

    public virtual void ReStartRound()
    {
        Debug.Log("Round : ReStartRound");

        _playerController.LoadRound();
    }

    public string GetExplanation()
    {
        return _explanation;
    }

    public Vector3 GetPlayerSpawn()
    {
        return _playerSpawn.localPosition;
    }

    public ROUNDTYPE GetRoundType()
    {
        return _roundType;
    }
}