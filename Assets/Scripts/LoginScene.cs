using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    // 로그인기능 완성시까지 로그인, 가입 버튼 비활성화
    // 바로 게임시작버튼으로 인게임화면 로비화면 진입
    [SerializeField] Transform _loginPopup;
    [SerializeField] Text _inputId;
    [SerializeField] Text _notiText;


    SceneLoadManager _scenemanager = null;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
    }

    public void OnClickPlayButton()
    {
        if (_inputId.text == string.Empty || _inputId.text == null)
        {
            _notiText.text = "ID를 입력하지 않았습니다.";
            return;
        }

        _notiText.text = "";
        GlobalData.id = _inputId.text;
        _loginPopup.gameObject.SetActive(false);
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }
}
