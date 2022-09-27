using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTimer : MonoBehaviour
{
    private bool isBattleStart = false;
    private DateTime _battleEndDateTime;

    // Update is called once per frame
    void Update()
    {
        if (false == isBattleStart)
            return;

        var remainTime = _battleEndDateTime - DateTime.UtcNow;
        if (remainTime.TotalSeconds >= 0)
            GetComponentInChildren<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}", remainTime.Minutes, remainTime.Seconds, remainTime.Milliseconds);
        else
            GetComponentInChildren<Text>().text = "00:00";

    }

    public void BattleStart(long gameStartDateTimeTicks, int gameSecond)
    {
        isBattleStart = true;
        _battleEndDateTime = new DateTime(gameStartDateTimeTicks).AddSeconds(gameSecond);
    }

    public void BattleEnd()
    {
        isBattleStart = false;
    }

    
}
