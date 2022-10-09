using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumDef;

public class IngameScene : MonoBehaviour
{
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
    [SerializeField] IngameUIController _uiController;
    [SerializeField] IngameLoadingController _loadingController;
    [SerializeField] IngameEndingController _endingController;

    public IngamePacketHandler PacketHandler { get { return _packetHandler; } }
    public IngameMapController MapController { get { return _mapController; } }
    public IngamePlayerController PlayerController { get { return _playerController; } }

    SceneLoadManager _scenemanager = null;

    INGAME_STATE _state = INGAME_STATE.LOADING;
    public bool IsWinner { get; set; } = false;

    private void Start()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();

        InitGame();
    }

    #region Init

    private void InitGame()
    {
        _state = INGAME_STATE.LOADING;
        _mapController.CreateMapAndRound();
        _playerController.CreatePlayer();
        _loadingController.LoadStartLoading();
    }

    public void CompleteStartLoading()
    {
        StartCoroutine(WaitInitGame());
    }

    private IEnumerator WaitInitGame()
    {
        while (true)
        {
            if (_mapController.CreateComplete && _playerController.CreateComplete)
                break;
            yield return null;
        }

        _packetHandler.SendInitGameComplete();
    }

    #endregion

    public void LoadRound()
    {
        _state = INGAME_STATE.PLAYING;
        _mapController.LoadRound(); // 라운드 셋팅 먼저
        _playerController.LoadRound();
        _uiController.LoadRound();
        _loadingController.LoadRoundLoading();
    }

    public void CompleteRoundLoading()
    {
        StartRound();
    }

    public void StartRound()
    {
        _mapController.StartRound();
        _playerController.StartRound();
        _uiController.StartRound();
    }

    public void ClearEnemyRound()
    {
        _uiController.SetRoundBar();
    }

    public void ClearGame(int score)
    {
        _state = INGAME_STATE.ENDING;
        _playerController.ClearGame();
        _uiController.ClearGame();
        _endingController.LoadEnding(score);
    }

    public void MoveLobbyScene()
    {
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }
}