using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round2 : Round
{
    [Header("Round 2")]
    [Space(10)]
    [SerializeField] Transform _keySpawn;
    [SerializeField] Transform _deads;
    [SerializeField] RoundObjClear _clear;

    private List<RoundObjKey> _keys;

    #region Base Round

    public override void LoadRound()
    {
        base.LoadRound();

        _keys = new List<RoundObjKey>();
        GameObject keyRes = Resources.Load<GameObject>("Prefabs/Round/Key");
        for (int i = 0; i < GlobalData.teamUserCount; ++i)
            _keys.Add(Instantiate(keyRes, _keySpawn.GetChild(i)).GetComponent<RoundObjKey>());

        RoundObjDead[] roundDeads = _deads.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in roundDeads)
            child.LoadRound(this);
        _clear.LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();

        foreach (var key in _keys)
            key.StartRound();
    }

    public override void SendClearRound()
    {
        if (_playerController.GetMyPlayer().HasKey)
            base.SendClearRound();
    }

    #endregion
}
