using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Round5 : Round
{
    [Header("Round 5")]
    [Space(10)]
    [SerializeField] Sprite _offFireImage;
    [SerializeField] Sprite _offButtonImage;
    [SerializeField] RoundObjKey[] _key;
    [SerializeField] RoundObjButton[] _buttonList = new RoundObjButton[3];
    [SerializeField] RoundObjClear _clear;

    #region Base Round

    public override void LoadRound()
    {
        base.LoadRound();

        _clear.LoadRound(this);
        for (int i = 0; i < _buttonList.Length; i++)
            _buttonList[i].LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();

        SetPlayerJumpHeight(1f);

        foreach (var key in _key)
            key.StartRound();

        for (int i = 0; i < _buttonList.Length; i++)
            _buttonList[i].StartRound();
    }

    public override void SendClearRound()
    {
        base.SendClearRound();
    }

    #endregion
}
