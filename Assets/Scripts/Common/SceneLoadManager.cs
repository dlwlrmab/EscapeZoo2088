using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [SerializeField] private Image _dim;  // 씬이동 or 맵 이동시 fadein/out 을 위한 이미지
    [SerializeField] private GameObject _loading;

    private float _fadeAnimTime = 1f;
    private float _fadeStart = 0f;
    private float _fadeEnd = 1f;
    private float _fadeTime = 0f;

    void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #region Loading

    private void SetLoading(bool isActive)
    {
        _loading.SetActive(isActive);
    }

    #endregion

    #region Fade In/Out

    // 점점 점점 어두워짐
    public void PlayFadeout(Action a = null, string sceneName = null)
    {
        StopAllCoroutines();
        StartCoroutine(CoFadeOut(a, sceneName));
    }

    // 점점 밝아짐
    public void PlayFadeIn(Action a = null, string sceneName = null)
    {
        StopAllCoroutines();
        StartCoroutine(CoFadeIn(a, sceneName));
    }

    private IEnumerator CoFadeOut(Action a, string sceneName)
    {
        Color color = _dim.color;
        _fadeTime = 0f;
        color.a = 0f;

        while (color.a < 1f)
        {
            _fadeTime += Time.deltaTime / _fadeAnimTime;
            color.a = Mathf.Lerp(_fadeStart, _fadeEnd, _fadeTime);
            _dim.color = color;
            yield return null;
        }

        if (a != null)
            a.Invoke();
        else if (sceneName != "" && sceneName != null)
            MoveScene(sceneName);

    }

    private IEnumerator CoFadeIn(Action a, string sceneName)
    {
        Color color = _dim.color;
        _fadeTime = 0f;
        color.a = 1f;

        while (color.a > 0f)
        {
            _fadeTime += Time.deltaTime / _fadeAnimTime;
            color.a = Mathf.Lerp(_fadeEnd, _fadeStart, _fadeTime);
            _dim.color = color;
            yield return null;
        }

        if (a != null)
            a.Invoke();
        else if (sceneName != "" && sceneName != null)
            MoveScene(sceneName);
    }

    #endregion
}