using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundObjClear : MonoBehaviour
{
    private Round _round;

    public void LoadRound(Round round)
    {
        _round = round;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.SetActive(false);
            _round.SendClearRound();
        }
    }
}