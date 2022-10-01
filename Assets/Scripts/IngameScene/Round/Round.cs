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
    }

    public virtual void ClearRound()
    {
        IngameScene.Instance.ClearRound();
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