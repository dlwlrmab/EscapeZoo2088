using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round4 : Round
{
    [Header("Round 4")]
    [Space(10)]
    [SerializeField] Transform _balls;
    [SerializeField] RoundObjClear _clear;
    [SerializeField] RoundObj _key;

    private RoundObjDead[] _roundDeads = null;

    #region Base Round

    public override void StartRound()
    {
        base.StartRound();

        _balls.gameObject.SetActive(true);

        _roundDeads = _balls.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in _roundDeads)
            child.SetRound(this);
        _clear.SetRound(this);
    }

    public override void ReStartRound()
    {
        base.ReStartRound();

        foreach (RoundObjDead child in _roundDeads)
        {
            var objCon = child.GetComponent<RoundObj>();
            if (objCon != null)
                objCon.Init();
        }

        _key.Init();
    }

    #endregion
}
