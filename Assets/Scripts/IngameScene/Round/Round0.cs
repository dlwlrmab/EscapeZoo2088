using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round0 : Round
{
    [Header("Round 0")]
    [Space(10)]
    [SerializeField] SpriteRenderer _sun;
    [SerializeField] Transform _deads;
    [SerializeField] RoundObjClear _clear;

    private List<Vector3> _prePlayerPos;
    private IEnumerator _coCheckPlayerMoving = null;

    #region Base Round

    public override void LoadRound()
    {
        base.LoadRound();

        _prePlayerPos = new List<Vector3>();

        RoundObjDead[] deads = _deads.GetComponentsInChildren<RoundObjDead>();
        foreach (RoundObjDead child in deads)
            child.LoadRound(this);
        _clear.LoadRound(this);
    }

    public override void StartRound()
    {
        base.StartRound();

        StartCoroutine(ShowSun());
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
        StartCoroutine(ShowSun());
    }

    #endregion

    private IEnumerator ShowSun()
    {
        _sun.transform.localPosition = new Vector3(-10, _sun.transform.localPosition.y);
        _sun.color = new Color(1, 1, 1, 0);

        while (true)
        {
            _sun.transform.localPosition += Vector3.right * Time.deltaTime * 5;
            _sun.color = new Color(1, 1, 1, _sun.color.a + Time.deltaTime * 0.7f);
            if (_sun.transform.localPosition.x >= 0)
            {
                yield return new WaitForSeconds(Random.Range(3, 5));
                break;
            }

            yield return null;
        }

        StartCoroutine(HideSun());
    }

    private IEnumerator HideSun()
    {
        _sun.transform.localPosition = new Vector3(0, _sun.transform.localPosition.y);
        _sun.color = new Color(1, 1, 1, 1);

        while (true)
        {
            _sun.transform.localPosition += Vector3.right * Time.deltaTime * 5;
            _sun.color = new Color(1, 1, 1, _sun.color.a - Time.deltaTime * 0.7f);
            if (_sun.transform.localPosition.x >= 10)
            {
                StartCoroutine(_coCheckPlayerMoving = CheckPlayerMoving());
                yield return new WaitForSeconds(Random.Range(3, 5));
                break;
            }

            yield return null;
        }

        StopCoroutine(_coCheckPlayerMoving);
        StartCoroutine(ShowSun());
    }

    private IEnumerator CheckPlayerMoving()
    {
        List<Player> players = _playerController.GetPlayerList();

        _prePlayerPos.Clear();
        foreach (var player in players)
            _prePlayerPos.Add(player.transform.localPosition);

        while (true)
        {
            players = _playerController.GetPlayerList();
            for (int i = 0; i < players.Count; ++i)
                if (players[i].transform.localPosition != _prePlayerPos[i])
                {
                    ReStartRound();
                    break;
                }

            yield return null;
        }
    }
}