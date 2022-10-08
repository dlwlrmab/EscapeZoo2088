using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class Player : MonoBehaviour
{
    private Transform _playerPerent;
    private PlayerInfo _info;
    public PlayerInfo Info { get; }

    public bool HasKey { get { return transform.Find("Key") != null; } } 

    void Start()
    {
        IngamePlayerController.Instance.AddPlayer(this);
        _playerPerent = transform.parent;
    }

    public void CreatePlayer(PlayerInfo playerInfo)
    {
        _info = playerInfo;
    }

    public void LoadRound(Vector3 startPos, Transform parent)
    {
        transform.position = startPos;
        transform.parent = parent;
    }

    void OnDestroy()
    {
        IngamePlayerController.Instance?.RemovePlayer(this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 땅이나 플레이어 위에 있는 경우는 점프 가능
        int layer = other.gameObject.layer;
        if ( layer == LayerMask.NameToLayer("Ground_Move"))
        {
            if (layer == LayerMask.NameToLayer("Ground_Move"))
                transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground_Move"))
        {
            transform.SetParent(_playerPerent);
        }
    }
}