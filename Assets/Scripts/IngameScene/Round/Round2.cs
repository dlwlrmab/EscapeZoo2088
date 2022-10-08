using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round2 : Round
{
    [Header("Round 2")]
    [Space(10)]
    [SerializeField] Transform _deads;
    [SerializeField] RoundObjClear _clear;

    #region Base Round

    public override void StartRound()
    {
        base.StartRound();

        RoundObjDead[] roundDeads = _deads.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in roundDeads)
            child.SetRound(this);
        _clear.SetRound(this);
    }

    public override void ClearRound(GameObject player)
    {
        base.ClearRound(player);
    }

    public override void ReStartRound()
    {
        base.ReStartRound();
    }

    #endregion
}
