using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameRoundController : MonoBehaviour
{
    [SerializeField] IngameScene _gameScene;
    [SerializeField] List<GameObject> _roundList;

    int _currentRound = 1;
    int _maxRound = 2; // 테스트를 위해 2개로 설정 , 최대 맵 갯수를 서버에서 내려줄지, 클라에서 고정으로 할지?

    public void OnLoadNextRound()
    {
        if (_currentRound >= _maxRound - 1)
        {
            _gameScene.ClearAllRound();
            _roundList[_currentRound].transform.parent.gameObject.SetActive(false);
        }
        else
        {
            if (_currentRound != -1)
            {
                _roundList[_currentRound++].SetActive(false);
                _roundList[_currentRound].SetActive(true);
            }
            else
                _roundList[++_currentRound].SetActive(true);
        }
    }
}
