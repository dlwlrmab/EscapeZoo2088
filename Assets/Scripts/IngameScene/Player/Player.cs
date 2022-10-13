using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;
using System;

public class Player : MonoBehaviour, IComparable
{
    private PlayerInfo _info;

    public bool IsMine
    {
        get
        {
            if (_info == null)
            {
                List<PlayerInfo> playerInfos = GlobalData.playerInfos;
                foreach (PlayerInfo info in playerInfos)
                {
                    if (info.Id == GlobalData.myId)
                        _info = info;
                }
            }

            return _info != null;
        }
    }

    public RoundObjKey HasKey { get { return GetComponentInChildren<RoundObjKey>(); } }

    void Start()
    {
        IngamePlayerController.Instance.AddPlayer(this);
    }

    public void StartRound(Vector3 startPos, Transform parent)
    {
        transform.position = startPos;
        transform.parent = parent;

        if (HasKey != null)
            Destroy(HasKey.gameObject);
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

    public ANIMAL GetAnimal()
    {
        return (ANIMAL)int.Parse(name.Replace("Player", "").Replace("(Clone)", ""));
    }
}