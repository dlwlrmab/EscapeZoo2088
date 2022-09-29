using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameLoadingController : MonoBehaviour
{
    [SerializeField] IngameScene _gameScene;
    [SerializeField] GameObject _loading;
    [SerializeField] Image _loadingBar;

    public void OnLoadStartLoading()
    {
        // 스토리 설명 로딩 시작
        StartCoroutine(ShowStartLoading());
    }

    public void OnLoadRoundLoading()
    {
        // 라운드 설명 로딩 시작
        StartCoroutine(ShowRoundLoading());
    }

    private IEnumerator ShowStartLoading()
    {
        _loading.SetActive(true);

        // 스토리 설명 추가

        var _time = 0f;
        _loadingBar.fillAmount = 0f;

        while (true)
        {
            yield return null;
            if (_loadingBar.fillAmount >= 1)
                break;

            _time += Time.deltaTime / 1;
            _loadingBar.fillAmount = Mathf.Lerp(0f, 1f, _time);
        }

        // 플레이어 입장 체크
        while (true)
        {
            break;
        }

        _loading.SetActive(false);
        _gameScene.CompleteStartLoading();
    }

    private IEnumerator ShowRoundLoading()
    {
        _loading.SetActive(true);

        // 라운드 설명 추가

        var _time = 0f;
        _loadingBar.fillAmount = 0f;

        while (true)
        {
            yield return null;
            if (_loadingBar.fillAmount >= 1)
                break;

            _time += Time.deltaTime / 1;
            _loadingBar.fillAmount = Mathf.Lerp(0f, 1f, _time);
        }

        // 맵 로딩 체크
        while (true)
        {
            break;
        }

        _loading.SetActive(false);
        _gameScene.CompleteRoundLoading();
    }
}
