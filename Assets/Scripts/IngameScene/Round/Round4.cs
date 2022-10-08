using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round4 : Round
{
    [Header("Round 4")]
    [Space(10)]
    [SerializeField] Transform _balls;
    [SerializeField] RoundObjKey _key;
    [SerializeField] RoundObjClear _clear;

    private RoundObjDead[] _roundDeads = null;

    #region Base Round

    public override void LoadRound()
    {
        base.LoadRound();

        _roundDeads = _balls.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in _roundDeads)
            child.LoadRound(this);
        _clear.LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();

        SetPlayerJumpHeight(1);

        _key.StartRound();
        foreach (RoundObjDead child in _roundDeads)
        {
            var objCon = child.GetComponent<RoundObj>();
            if (objCon != null)
                objCon.StartRound();
        }

        _balls.gameObject.SetActive(true);
    }

    public override void ClearRound()
    {
        if (_playerController.GetMyPlayer().HasKey)
            base.ClearRound();
    }

    public override void ReStartRound()
    {
        base.ReStartRound();
    }

    #endregion
}
