using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager _instance = null;  

    public static SceneLoadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                System.Type tType = typeof(SceneLoadManager);
                _instance = FindObjectOfType(tType) as SceneLoadManager;

                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<SceneLoadManager>();
                    _instance.gameObject.name = tType.Name;
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }
    }


    [SerializeField] Image _dim;  // 씬이동 or 맵 이동시 fadein/out 을 위한 이미지

    float _animTime = 1f;
    float _start = 0f;
    float _end = 1f;
    float _time = 0f;

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
        _time = 0f;
        color.a = 0f;

        while (color.a < 1f)
        {
            _time += Time.deltaTime / _animTime;
            color.a = Mathf.Lerp(_start, _end, _time);
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
        _time = 0f;
        color.a = 1f;

        while (color.a > 0f)
        {
            _time += Time.deltaTime / _animTime;
            color.a = Mathf.Lerp(_end, _start, _time);
            _dim.color = color;
            yield return null;
        }

        if (a != null)
            a.Invoke();
        else if (sceneName != "" && sceneName != null)
            MoveScene(sceneName);
    }

    void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}