using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round3 : Round
{
    [Header("Round 3")]
    [Space(10)]
    [SerializeField] RoundClear _clear;

    #region Base Round

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

    #endregion
}
