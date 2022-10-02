using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Round5 : Round
{
    [Header("Round 5")]
    [Space(10)]
    [SerializeField] RoundObjClear _clear;
    [SerializeField] RoundObjButton[] _buttonList = new RoundObjButton[3];
    [SerializeField] Sprite _offFireImage;
    [SerializeField] Sprite _offButtonImage;

    #region Base Round

    public override void StartRound()
    {
        base.StartRound();

        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].SetData(this);
            _buttonList[i].Init();
        }

        _clear.SetRound(this);
    }

    public override void ReStartRound()
    {
        base.ReStartRound();

        foreach (RoundObjButton button in _buttonList)
            button.Init();
    }

    #endregion
}
