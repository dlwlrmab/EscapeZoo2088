using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round2 : Round
{
    [Header("Round 2")]
    [Space(10)]
    [SerializeField] Transform _deadMap;
    [SerializeField] RoundClear _clear;

    private RoundDead[] _roundDeads = null;

    #region Base Round

    public override void StartRound()
    {
        base.StartRound();

        _roundDeads = _deadMap.GetComponentsInChildren<RoundDead>();
        foreach (RoundDead child in _roundDeads)
            child.SetRound(this);
        _clear.SetRound(this);
    }

    public override void ClearRound(GameObject player)
    {
        base.ClearRound(player);

        StopAllCoroutines();
    }

    public override void ReStartRound()
    {
        base.ReStartRound();
    }

    #endregion
}
