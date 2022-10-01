using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] protected string _explanation = "";

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

    public string GetExplanation()
    {
        return _explanation;
    }

    public Vector3 GetPlayerSpawn()
    {
        return _playerSpawn.localPosition;
    }
}