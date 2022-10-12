using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundObjKey : MonoBehaviour
{
    private Vector3 _startPos;
    private Transform _parent;

    private void Awake()
    {
        _startPos = transform.position;
        _parent = transform.parent;
    }

    public void StartRound()
    {
        transform.position = _startPos;
        if (_parent != null)
            transform.parent = _parent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.HasKey != null)
                return;

            transform.parent = other.transform;
            transform.localPosition = new Vector3(0.5f, 0.7f, 0);
        }
    }
}