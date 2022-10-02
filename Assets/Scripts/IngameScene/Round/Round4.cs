using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round4 : Round
{
    [Header("Round 4")]
    [Space(10)]
    [SerializeField] Transform _balls;
    [SerializeField] RoundClear _clear;
    [SerializeField] ObjectController _key;

    private RoundDead[] _roundDeads = null;

    #region Base Round

    public override void StartRound()
    {
        base.StartRound();

        _balls.gameObject.SetActive(true);

        _roundDeads = _balls.GetComponentsInChildren<RoundDead>();
        foreach (RoundDead child in _roundDeads)
            child.SetRound(this);
        _clear.SetRound(this);
    }

    public override void ReStartRound()
    {
        base.ReStartRound();

        foreach (RoundDead child in _roundDeads)
        {
            var objCon = child.GetComponent<ObjectController>();
            if (objCon != null)
                objCon.Init();
        }

        _key.Init();
    }

    #endregion
}
