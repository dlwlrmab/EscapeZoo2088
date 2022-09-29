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

    [Header("Controller")]
    [SerializeField] IngameLoadingController _loadingController;
    [SerializeField] IngameEndingController _endingController;
    [SerializeField] IngameMapController _mapController; // 맵, 오브젝트의 스킨을 결정하는 컨트롤러
    [SerializeField] IngameRoundController _roundController; // 라운드를 관리하는 컨트롤러

    SceneLoadManager _scenemanager = null;

    STATE _state = STATE.LOADING;
    List<Player> _playerList;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();

        _state = STATE.LOADING;
        _loadingController.OnLoadStartLoading();
    }

    public void CompleteStartLoading()
    {
        _state = STATE.PLAYING;
        _loadingController.OnLoadRoundLoading();
        _roundController.OnLoadNextRound();
    }

    public void CompleteRoundLoading()
    {
        // 게임 시작
    }

    public void ClearRound()
    {
        // 서버로 해당맵 클리어를 알림
        // 서버로 _currentMapCnt 주면될듯?

        // 서버에게 클리어 알림 후 바로 받았다고 가정
        RecvClearMap();
    }

    public void RecvClearMap()
    {
        // 서버로부터 클리어알림에대한 응답을 받음
        // 다음맵으로 이동

        _loadingController.OnLoadRoundLoading();
        _roundController.OnLoadNextRound();
    }

    public void ClearAllRound()
    {
        _state = STATE.ENDING;
        _endingController.OnLoadEnding();
    }

    void OnLoadLobbyScene()
    {
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }

    #region Test

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && _state == STATE.PLAYING)
        {
            ClearRound();
        }
        else if (Input.GetKeyDown(KeyCode.S) && _state == STATE.ENDING)
        {
            OnLoadLobbyScene();
        }
    }

    #endregion
}