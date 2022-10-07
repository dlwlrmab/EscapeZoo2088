using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameLoadingController : MonoBehaviour
{
    [SerializeField] GameObject _loading;
    [SerializeField] Text _explanation;
    [SerializeField] Image _loadingBar;

    public void LoadStartLoading()
    {
        // 스토리 설명 로딩 시작
        StartCoroutine(ShowStartLoading());
    }

    private IEnumerator ShowStartLoading()
    {
        _loading.SetActive(true);
        _loadingBar.transform.parent.gameObject.SetActive(true);

        _explanation.text = "스토리 설명 및 플레이어 입장중";
        _loadingBar.fillAmount = 0f;

        var time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            _loadingBar.fillAmount = Mathf.Lerp(0f, 1f, time);
            if (_loadingBar.fillAmount >= 1)
                break;

            yield return null;
        }

        _loading.SetActive(false);
        _loadingBar.transform.parent.gameObject.SetActive(false);
        IngameScene.Instance.CompleteStartLoading();
    }

    public void LoadRoundLoading()
    {
        // 라운드 설명 로딩 시작
        StartCoroutine(ShowRoundLoading());
    }

    private IEnumerator ShowRoundLoading()
    {
        _loading.SetActive(true);
        _loadingBar.transform.parent.gameObject.SetActive(false);

        _explanation.text = IngameScene.Instance.MapController.GetExplanation();

        var time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= 3)
                break;

            yield return null;
        }

        _loading.SetActive(false);
        IngameScene.Instance.CompleteRoundLoading();
    }
}