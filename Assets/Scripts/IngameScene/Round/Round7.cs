using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Round7 : Round
{
    [SerializeField] RoundClear _clear;

    [SerializeField] ButtonObj[] _buttonList = new ButtonObj[3];
    [SerializeField] Sprite _offFireImage;
    [SerializeField] Sprite _offButtonImage;
    public override void StartRound()
    {
        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].SetData(this);
            _buttonList[i].Init();
        }
        _clear.SetRound(this);

        base.StartRound();
    }

    public override void ReStartRound()
    {
        foreach (ButtonObj button in _buttonList)
            button.Init();

        base.ReStartRound();
    }

}
