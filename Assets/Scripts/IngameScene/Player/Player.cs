using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;
using System;

public class Player : MonoBehaviour, IComparable
{
    private PlayerInfo _info;
    public PlayerInfo Info { get; }

    public bool HasKey { get { return transform.Find("Key") != null; } }

    void Start()
    {
        IngamePlayerController.Instance.AddPlayer(this);

        List<PlayerInfo> playerInfos = GlobalData.playerInfos;
        foreach (PlayerInfo info in playerInfos)
        {
            if (info.Id == GlobalData.myId)
            {
                _info = info;
                return;
            }
        }
    }

    public void LoadRound(Vector3 startPos, Transform parent)
    {
        gameObject.SetActive(true);

        transform.position = startPos;
        transform.parent = parent;
    }

    void OnDestroy()
    {
        IngamePlayerController.Instance?.RemovePlayer(this);
    }

    public int CompareTo(object obj)
    {
        var player = obj as Player;
        // 플레이어 이름순으로 정렬
        return String.Compare(this.name, player.name);
    }
}