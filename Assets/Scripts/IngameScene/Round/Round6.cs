using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Round6 : Round
{
    [Header("Round 6")]
    [Space(10)]
    [SerializeField] RoundObjClear _clear;
    [SerializeField] RoundObj _key;
    [SerializeField] RoundObjButton[] _buttonList = new RoundObjButton[3];

    #region Base Round

    public override void StartRound()
    {
        base.StartRound();
        _clear.SetRound(this);


        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].SetData(this);
            _buttonList[i].Init();
        }
    }

    public override void ClearRound(GameObject player)
    {
        if (player != null)
        {
            var p = player.GetComponent<Player>();

            if (p != null)
            {
                if (p.HasKey)
                {
                    base.ClearRound(player);
                    _key.Init();
                }
            }
        }
       
    }
    public override void ReStartRound()
    {
        base.ReStartRound();
        _key.Init();
    }
    #endregion
}
