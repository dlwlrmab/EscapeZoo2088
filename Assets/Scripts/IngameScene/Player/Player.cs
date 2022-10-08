using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class Player : MonoBehaviour
{
    private PlayerInfo _info;

    public bool HasKey { get { return transform.Find("Key") != null; } } 

    void Start()
    {
        IngamePlayerController.Instance.AddPlayer(this);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        var obj = other.gameObject.GetComponent<RoundObj>();
        if (obj != null)
        {
            var type = obj.GetObjectType();
            if (type == BLOCKTYPE.KEY)
            {
                var parent = other.transform.parent;
                other.transform.SetParent(transform);
                other.transform.localPosition = new Vector3(0f, 0.7f, 0);
            }
        }
    }

    void OnDestroy()
    {
        IngamePlayerController.Instance?.RemovePlayer(this);
    }
}