using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundObjKey : MonoBehaviour
{
    private Transform _parent;

    private void Awake()
    {
        _parent = transform.parent;
    }

    public void StartRound()
    {
        if (_parent != null)
            transform.parent = _parent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.HasKey)
                return;                

            transform.parent = other.transform;
            transform.localPosition = new Vector3(0f, 0.7f, 0);
        }
    }
}