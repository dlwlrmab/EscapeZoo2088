using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    // 로그인기능 완성시까지 로그인, 가입 버튼 비활성화
    // 바로 게임시작버튼으로 인게임화면 로비화면 진입
    [SerializeField] Transform _loginButton;
    [SerializeField] Text _inputId;
    [SerializeField] Text _notiText;


    SceneLoadManager _scenemanager = null;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
    }

    public void OnClickLoginButton()
    {
        if (_inputId.text == string.Empty || _inputId.text == null)
        {
            _notiText.text = "ID를 입력하지 않았습니다.";
            return;
        }

        // 서버 작업 이후 수정되어야할 코드들
        //Login();
        RecvLoginResult(true);
    }

    // 서버로 로그인 정보 보냄
    void Login()
    {

    }

    // 서버로부터 로그인 결과를 받아서 처리
    // 필요한 데이터 
    // 로그인 성공여부, 플레이가능한 라운드?
    public void RecvLoginResult(bool success)
    {
        if (success)
            _notiText.text = "로그인 성공";
        else
            _notiText.text = "로그인 실패";

        GlobalData.id = _inputId.text;
        _loginButton.gameObject.SetActive(false);
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }
}