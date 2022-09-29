﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameScene : MonoBehaviour
{
    enum STATE
    {
        LOADING = 0,
        PLAYING,
        ENDING
    }

    public static IngameScene _instance = null;

    public static IngameScene Instance
    {
        get
        {
            if (_instance == null)
            {
                System.Type tType = typeof(IngameScene);
                _instance = FindObjectOfType(tType) as IngameScene;

                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<IngameScene>();
                    _instance.gameObject.name = tType.Name;
                }
            }
            return _instance;
        }
    }

    [Header("Controller")]
    [SerializeField] IngamePacketHandler _packetHandler;
    [SerializeField] IngameMapController _mapController;
    [SerializeField] IngameLoadingController _loadingController;
    [SerializeField] IngameEndingController _endingController;

    SceneLoadManager _scenemanager = null;

    STATE _state = STATE.LOADING;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();

        _state = STATE.LOADING;
        _loadingController.OnLoadStartLoading();
    }

    public void CompleteStartLoading()
    {
        StartCoroutine(WaitLoadingComplete());
    }

    private IEnumerator WaitLoadingComplete()
    {
        while (true)
        {
            // 맵, 라운드들, 플레이어 등 생성 완료
            if (_mapController.CreateComplete)
                break;

            yield return null;
        }

        _packetHandler.SendGameLoadingComplete();
    }

    public void StartRound()
    {
        // 진짜 게임 시작

        Invoke("ClearRound", 3);
    }

    public void ClearRound()
    {
        _packetHandler.SendRoundClear();
    }

    public void OnLoadLobbyScene()
    {
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }

    #region IngamePacketHandler

    public void RecvEnterGame(int[] roundList)
    {
        _mapController.CreateMapAndRound(roundList);
    }

    public void RecvRoundStart(int nextRound)
    {
        _state = STATE.PLAYING;
        _mapController.OnLoadNextRound(nextRound);
        _loadingController.OnLoadRoundLoading();
    }

    public void RecvGameResult(int rank)
    {
        _state = STATE.ENDING;
        _endingController.OnLoadEnding(rank);
    }

    #endregion
}