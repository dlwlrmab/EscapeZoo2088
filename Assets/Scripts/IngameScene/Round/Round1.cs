using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round1 : Round
{
    [Header("Round 1")]
    [Space(10)]
    [SerializeField] Transform _dropMap;
    [SerializeField] GameObject _emptyGound;
    [SerializeField] RoundClear _clear;

    private RoundDead[] _roundDeads = null;

    #region Base Round

    public override void StartRound()
    {
        base.StartRound();

        _roundDeads = _dropMap.GetComponentsInChildren<RoundDead>();
        foreach (RoundDead child in _roundDeads)
            child.SetRound(this);
        _clear.SetRound(this);

        StartCoroutine(Drop());
    }

    public override void ClearRound()
    {
        base.ClearRound();

        StopAllCoroutines();
    }

    public override void ReStartRound()
    {
        base.ReStartRound();

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
