using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameScene : MonoBehaviour
{
    enum SceneState
    {
        loading = 0,
        playing = 1,
        ending = 2,
    }

    [SerializeField] Image _loadingBar;
    [SerializeField] GameObject _loadingScene;
    [SerializeField] List<GameObject> _mapList; // 추후 맵 생성방식에따라 변경
    [SerializeField] List<Player> _playerList;
    [SerializeField] GameObject _ending;
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _lose;

    SceneLoadManager _scenemanager = null;
    SceneState _state = SceneState.loading;
    int _currentRound = 1;
    int _currentMapCnt = -1; // 현재 맵 index
    int _maxStage = 2; // 테스트를 위해 2개로 설정 , 최대 맵 갯수를 서버에서 내려줄지, 클라에서 고정으로 할지?

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
        OnLoadingScene();
    }

    private void Update()
    {
        // 테스트용 코드
        if (Input.GetKeyDown(KeyCode.A) && _state == SceneState.playing)
        {
            NextMap();
        }
        else if (Input.GetKeyDown(KeyCode.S) && _state == SceneState.ending)
        {
            OnLoadLobbyScene();
        }
    }

    // 서버로 해당맵 클리어를 알림
    public void ClearMap()
    {
        // 서버로 _currentMapCnt 주면될듯?
    }

    // 서버로부터 클리어알림에대한 응답을 받음
    public void RecvClearMap()
    {
        // 다음맵으로 이동
        NextMap();
    }

    // 게임 진입시 로딩화면
    void OnLoadingScene()
    {
        StartCoroutine(CoMapLoading());
    }

    void OnLoadingNextMap()
    {
        if (_currentMapCnt >= _maxStage - 1)
        {
            OnLoadEndingScene();
        }
        else
        {
            if (_currentMapCnt != -1)
            {
                _mapList[_currentMapCnt++].SetActive(false);
                _mapList[_currentMapCnt].SetActive(true);
            }
            else
                _mapList[++_currentMapCnt].SetActive(true);
        }
    }

    void OnLoadEndingScene()
    {
        int a = Random.Range(0, 3);
        // 승리
        if (a == 0)
        {
            _win.SetActive(true);
        }
        // 패배
        else
        {
            _lose.SetActive(true);
        }

        _state = SceneState.ending;
        _mapList[_currentMapCnt].transform.parent.gameObject.SetActive(false);
    }

    void OnLoadLobbyScene()
    {
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }

    // 이때 사용할 맵 프리팹 모두 받아두기
    private IEnumerator CoMapLoading()
    {
        _state = SceneState.loading;
        _loadingScene.SetActive(true);

        _loadingBar.fillAmount = 0f;
        var _time = 0f;

        while (true)
        {
            yield return null;
            if (_loadingBar.fillAmount >= 1)
                break;

            _time += Time.deltaTime / 1;
            _loadingBar.fillAmount = Mathf.Lerp(0f, 1f, _time);
        }

        NextMap();
    }

    void NextMap()
    {
        _scenemanager.PlayFadeout(() =>
        {
            _state = SceneState.playing;
            _loadingScene.SetActive(false);
            OnLoadingNextMap();

            _scenemanager.PlayFadeIn();
        });
    }



}