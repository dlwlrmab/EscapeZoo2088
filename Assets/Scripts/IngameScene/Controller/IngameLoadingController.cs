using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameLoadingController : MonoBehaviour
{
    [Header("Start Loading")]
    [SerializeField] GameObject _startLoading;
    [SerializeField] RectTransform _startBackground;
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

        _startBackground.localScale = Vector3.one;
        _loadingBar.fillAmount = 0f;

        var time = 0f;
        while (true)
        {
            time += Time.deltaTime;

            if (_startBackground.localScale.x <= 0.7f)
                _startBackground.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            else
                _startBackground.localScale = new Vector3(_startBackground.localScale.x - time * 0.0001f, _startBackground.localScale.y - time * 0.0001f, 1);

            _loadingBar.fillAmount = Mathf.Lerp(0f, 1f, time * 0.3f);
            if (_loadingBar.fillAmount >= 1)
                break;

            yield return null;
        }

        IngameScene.Instance.CompleteStartLoading();
    }

    public void HideStartLoading()
    {
        _startLoading.SetActive(false);
        _loadingBar.transform.parent.gameObject.SetActive(false);
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