using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameLoadingController : MonoBehaviour
{
    [Header("Start Loading")]
    [SerializeField] GameObject _startLoading;
    [SerializeField] Image _loadingBar;

    [Header("Round Loading")]
    [SerializeField] GameObject _roundLoading;
    [SerializeField] Text _roundExplanation;

    #region Start Loading

    public void LoadStartLoading()
    {
        // 스토리 설명 로딩 시작
        StartCoroutine(ShowStartLoading());
    }

    private IEnumerator ShowStartLoading()
    {
        _startLoading.SetActive(true);
        _roundLoading.SetActive(false);

        _loadingBar.fillAmount = 0f;

        var time = 0f;
        while (true)
        {
            time += Time.deltaTime * 0.3f;
            _loadingBar.fillAmount = Mathf.Lerp(0f, 1f, time);
            if (_loadingBar.fillAmount >= 1)
                break;

            yield return null;
        }

        _startLoading.SetActive(false);
        _loadingBar.transform.parent.gameObject.SetActive(false);
        IngameScene.Instance.CompleteStartLoading();
    }

    #endregion

    #region Round Loading

    public void LoadRoundLoading()
    {
        // 라운드 설명 로딩 시작
        StartCoroutine(ShowRoundLoading());
    }

    private IEnumerator ShowRoundLoading()
    {
        _startLoading.SetActive(false);
        _roundLoading.SetActive(true);

        _roundExplanation.text = IngameScene.Instance.MapController.GetExplanation();

        var time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= 3)
                break;

            yield return null;
        }

        _roundLoading.SetActive(false);
        IngameScene.Instance.CompleteRoundLoading();
    }

    #endregion
}