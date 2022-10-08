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

    public override void LoadRound()
    {
        base.LoadRound();

        RoundObjDead[] roundDeads = _deads.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in roundDeads)
            child.LoadRound(this);
        _clear.LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();
    }

    public override void ClearRound()
    {
        base.ClearRound();
    }

    public override void ReStartRound()
    {
        base.ReStartRound();
    }

    #endregion
}
