using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round3 : Round
{
    [Header("Round 3")]
    [Space(10)]
    [SerializeField] RoundObjButton[] _buttons;
    [SerializeField] Transform _deads;
    [SerializeField] RoundObjClear _clear;

    RoundObj[] _roundObjs;

    #region Base Round

    public override void LoadRound()
    {
        base.LoadRound();

        _roundObjs = GetComponentsInChildren<RoundObj>();

        RoundObjDead[] roundDeads = _deads.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in roundDeads)
            child.LoadRound(this);
        _clear.LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();

        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].StartRound();
        for (int i = 0; i < _roundObjs.Length; i++)
            _roundObjs[i].StartRound();
    }

    #endregion
}