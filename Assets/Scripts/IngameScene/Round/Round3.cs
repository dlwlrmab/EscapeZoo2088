using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round3 : Round
{
    [Header("Round 3")]
    [Space(10)]
    [SerializeField] RoundObjButton[] _fireButtons;
    [SerializeField] RoundObjButton[] _ballButtons;
    [SerializeField] Transform _balls;
    [SerializeField] Transform _deads;
    [SerializeField] RoundObjClear _clear;

    RoundObjDead[] _roundDeads;

    #region Base Round

    public override void LoadRound()
    {
        base.LoadRound();

        for (int i = 0; i < _fireButtons.Length; i++)
            _fireButtons[i].LoadRound(this);
        for (int i = 0; i < _ballButtons.Length; i++)
            _ballButtons[i].LoadRound(this);

        _roundDeads = _deads.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in _roundDeads)
            child.LoadRound(this);
        _clear.LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();

        for (int i = 0; i < _fireButtons.Length; i++)
            _fireButtons[i].StartRound();
        for (int i = 0; i < _ballButtons.Length; i++)
            _ballButtons[i].StartRound();

        foreach (RoundObjDead child in _roundDeads)
        {
            var objCon = child.GetComponent<RoundObj>();
            if (objCon != null)
                objCon.StartRound();
        }

        #endregion
    }
}