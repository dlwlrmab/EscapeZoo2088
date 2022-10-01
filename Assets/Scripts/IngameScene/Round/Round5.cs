using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round5 : Round
{
    [SerializeField] RoundClear _clear;

    public override void StartRound()
    {
        base.StartRound();
        _clear.SetRound(this);
    }


    public override void ClearRound(GameObject player)
    {
        if (player != null)
        {
            var p = player.GetComponent<Player>();

            if (p != null)
            { 
                if (p.HasKey)
                {
                    base.ClearRound(player);
                }
            }
        }
    }
}
