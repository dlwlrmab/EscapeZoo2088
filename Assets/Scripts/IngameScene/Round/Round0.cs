﻿using System.Collections;
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

    private float SUN_FADEIN_TIME = 2f;
    private float SUN_FADEOUT_TIME = 3f;
    private float SUN_SHOW_TIME = 3f;

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

        StartSun();
    }

    public override void UpdateRound()
    {
        base.UpdateRound();

        StartSun();
    }

    #endregion

    public void StartSun()
    {
        StopAllCoroutines();
        StartCoroutine(ShowSun());
    }

    private IEnumerator ShowSun()
    {
        var time = 0f;
        var startPos = new Vector3(-10, _sun.transform.localPosition.y);
        var endPos = new Vector3(0, _sun.transform.localPosition.y);
        while (true)
        {
            time += Time.deltaTime / SUN_FADEIN_TIME;

            _sun.color = new Color(1, 1, 1, Mathf.Lerp(0f, 1f, time));
            _sun.transform.position = Vector3.Lerp(startPos, endPos, time);

            if (time >= 1)
            {
                yield return new WaitForSeconds(SUN_SHOW_TIME);
                break;
            }

            yield return null;
        }

        StartCoroutine(HideSun());
    }

    private IEnumerator HideSun()
    {
        var time = 0f;
        var startPos = new Vector3(0, _sun.transform.localPosition.y);
        var endPos = new Vector3(10, _sun.transform.localPosition.y);
        while (true)
        {
            time += Time.deltaTime / SUN_FADEOUT_TIME;

            _sun.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, time));
            _sun.transform.position = Vector3.Lerp(startPos, endPos, time);

            if (time >= 1)
            {
                StartCoroutine(CheckPlayerMoving());
                break;
            }

            yield return null;
        }

        Invoke("StartSun", GlobalData.SunriseTime);
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
                    SendReStartRound();
                    break;
                }

            yield return null;
        }
    }
}