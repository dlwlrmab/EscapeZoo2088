using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameLoadingController : MonoBehaviour
{
    [SerializeField] GameObject _loading;
    [SerializeField] Image _loadingBar;

    public void OnLoadStartLoading()
    {
        // 스토리 설명 로딩 시작
        StartCoroutine(ShowStartLoading());
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

        _loading.SetActive(false);
        IngameScene.Instance.CompleteStartLoading();
    }

    public void OnLoadRoundLoading()
    {
        // 맵 컨트롤러한테 맵 라운드 설명 받아오기
        // 라운드 설명 로딩 시작
        StartCoroutine(ShowRoundLoading());
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

        _loading.SetActive(false);
        IngameScene.Instance.StartRound();
    }
}
