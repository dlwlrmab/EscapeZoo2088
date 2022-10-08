using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round1 : Round
{
    [Header("Round 1")]
    [Space(10)]
    [SerializeField] GameObject _emptyGound;
    [SerializeField] Transform _dropMap;
    [SerializeField] Transform _deads;
    [SerializeField] RoundObjClear _clear;

    #region Base Round

    public override void LoadRound()
    {
        base.LoadRound();

        RoundObjDead[] roundDeads = _deads.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in roundDeads)
            child.LoadRound(this);
        _clear.LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();

        StopAllCoroutines();
        StartCoroutine(Drop());
    }

    #endregion

    private IEnumerator Drop()
    {
        _emptyGound.SetActive(true);
        _dropMap.transform.localPosition = Vector3.zero;
        float clearPosY = -_clear.transform.localPosition.y - 5;

        while (true)
        {
            _dropMap.transform.localPosition += Vector3.up * Time.deltaTime * 10;
            if (_dropMap.transform.localPosition.y >= clearPosY)
            {
                _emptyGound.SetActive(false);
                break;
            }

            yield return null;
        }
    }
}
