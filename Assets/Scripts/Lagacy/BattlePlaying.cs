using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlaying : MonoBehaviour
{
    public GameObject BattleTimerPanelPrefab;
    private GameObject _battleTimer;

    public void BattleStart(long gameStartDateTimeTicks, int gameSecond)
    {
        _battleTimer = Instantiate(BattleTimerPanelPrefab, transform);
        _battleTimer.GetComponent<BattleTimer>().BattleStart(gameStartDateTimeTicks, gameSecond);
    }

    public void RoundStart(long gameStartDateTimeTicks, int gameSecond)
    {
        _battleTimer.GetComponent<BattleTimer>().BattleStart(gameStartDateTimeTicks, gameSecond);
    }

    public void BattleEnd()
    {
        _battleTimer.GetComponent<BattleTimer>().BattleEnd();
    }
}
