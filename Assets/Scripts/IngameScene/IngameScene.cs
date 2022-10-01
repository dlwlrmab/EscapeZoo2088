using System.Collections;
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
    [SerializeField] IngamePlayerController _playerController;
    [SerializeField] IngameLoadingController _loadingController;
    [SerializeField] IngameEndingController _endingController;

    public IngameMapController MapController { get { return _mapController; } }
    public IngamePlayerController PlayerController { get { return _playerController; } }

    SceneLoadManager _scenemanager = null;

    STATE _state = STATE.LOADING;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();

        _state = STATE.LOADING;
        _loadingController.LoadStartLoading();
    }

    public void CompleteStartLoading()
    {
        StartCoroutine(WaitCreation());
    }

    private IEnumerator WaitCreation()
    {
        while (true)
        {
            if (_mapController.CreateComplete && _playerController.CreateComplete)
                break;
            yield return null;
        }

        _packetHandler.SendGameLoadingComplete();
    }

    public void StartRound()
    {
        _playerController.StartRound();
        _mapController.StartRound();
    }

    public void ClearRound()
    {
        _packetHandler.SendRoundClear();
    }

    public void MoveLobbyScene()
    {
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }

    #region IngamePacketHandler

    public void RecvEnterGame(int[] roundList)
    {
        _mapController.CreateMapAndRound(roundList);
        _playerController.CreatePlayer();
    }

    public void RecvRoundStart(int nextRound)
    {
        _state = STATE.PLAYING;
        _mapController.LoadRound(nextRound); // 맵 세팅 먼저
        _playerController.LoadRound();
        _loadingController.LoadRoundLoading();
    }

    public void RecvGameResult(int rank)
    {
        _state = STATE.ENDING;
        _endingController.LoadEnding(rank);
    }

    #endregion
}