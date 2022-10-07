using Assets.Scripts;
using CommonProtocol;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class LoginScene : MonoBehaviour
{
    [SerializeField] Transform _startCanvas;
    [SerializeField] Transform _Canvas;
    [SerializeField] Transform _loginCanvas;
    [SerializeField] Transform _joinCanvas;

    // 로그인기능 완성시까지 로그인, 가입 버튼 비활성화
    // 바로 게임시작버튼으로 인게임화면 로비화면 진입
    [SerializeField] Transform _loginButton;
    [SerializeField] Transform _joinButton;

    [SerializeField] Text _inputId;
    [SerializeField] Text _inputPW;
    [SerializeField] Text _inputMBTI;
    [SerializeField] Text _notiText;

    SceneLoadManager _scenemanager = null;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
        _Canvas.gameObject.SetActive(false);
        _startCanvas.gameObject.SetActive(true);
    }

    #region button
    public void OnClickGoLogin()
    {
        _notiText.text = "로그인";

        _startCanvas.gameObject.SetActive(false);
        _Canvas.gameObject.SetActive(true);
        _loginCanvas.gameObject.SetActive(true);
        _loginButton.gameObject.SetActive(true);
        _joinButton.gameObject.SetActive(false);
        _inputId.transform.parent.gameObject.SetActive(true);
        _inputPW.transform.parent.gameObject.SetActive(true);
        _inputMBTI.transform.parent.gameObject.SetActive(false);
    }

    public void OnClickGoJoin()
    {
        _notiText.text = "회원가입";

        _startCanvas.gameObject.SetActive(false);
        _Canvas.gameObject.SetActive(true);
        _joinCanvas.gameObject.SetActive(true);
        _loginButton.gameObject.SetActive(false);
        _joinButton.gameObject.SetActive(true);
        _inputId.transform.parent.gameObject.SetActive(true);
        _inputPW.transform.parent.gameObject.SetActive(true);
        _inputMBTI.transform.parent.gameObject.SetActive(true);
    }

    public void OnClickExit()
    {
        _startCanvas.gameObject.SetActive(true);
        _Canvas.gameObject.SetActive(false);
        initText();
    }

    public void OnClickLoginButton()
    {
        string id = _inputId.text;
        string pw = _inputPW.text;

        if (id == string.Empty || id == null)
        {
            _notiText.text = "ID를 입력하지 않았습니다.";
            return;
        }

        if (pw == string.Empty || pw == null)
        {
            _notiText.text = "PW를 입력하지 않았습니다.";
            return;
        }

        // 서버 작업 이후 수정되어야할 코드들
        Login(id, pw);
    }

    public void OnClickJoinButton()
    {
        string id = _inputId.text;
        string pw = _inputPW.text;
        string mbti = _inputMBTI.text;

        if (id == string.Empty || id == null)
        {
            _notiText.text = "ID를 입력하지 않았습니다.";
            return;
        }

        if (pw == string.Empty || pw == null)
        {
            _notiText.text = "PW를 입력하지 않았습니다.";
            return;
        }

        if (!CheckMBTI(mbti))
        {
            _notiText.text = "잘못된 MBTI 입니다.";
            return;
        }

        // 서버 작업 이후 수정되어야할 코드들
        ReqJoinAccount(id, pw, mbti);
    }
    #endregion

    #region reqServer
    // 서버로 로그인 정보 보냄
    void Login(string id, string pw)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(pw))
            return;

        var req = new ReqLogin
        {
            userId = id,
            password = pw,
        };

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "Login", (a) => {
            RecvLoginResult(a);
        }));
    }

    void ReqJoinAccount(string id, string pw, string mbti)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        var req = new ReqAccountJoin
        {
            userId = id,
            password = pw,
            mbti = mbti,
        };

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "AccountJoin", (a) =>
        {
            RecvJoinResult(a);
        }));
    }
    #endregion

    #region resServer
    // 서버로부터 로그인 결과를 받아서 처리
    // 필요한 데이터 
    // 플레이가능한 라운드?
    public void RecvLoginResult(string responseString)
    {
        var res = JsonConvert.DeserializeObject<ResLogin>(responseString);

        if (res.ResponseType == ResponseType.Fail)
        {
            _notiText.text = "ID / PW 를 확인해 주세요.";
            return;
        }
        else
        {
            _notiText.text = "로그인 성공";
            GlobalData.id = res.userId;
            initText();
        }


        GlobalData.id = _inputId.text;
        GlobalData.mbti = _inputMBTI.text.ToUpper();

        _loginButton.gameObject.SetActive(false);
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }

    public void RecvJoinResult(string responseBytes)
    {
        var res = JsonConvert.DeserializeObject<ResAccountJoin>(responseBytes);

        if (res.ResponseType == ResponseType.DuplicateName)
        {
            _notiText.text = "이미 존재하는 ID입니다.";
        }
        else if (res.ResponseType == ResponseType.Success)
        {
            _notiText.text = "회원가입 성공, 로그인 해주세요";
            initText();
        }
        else
        {
            _notiText.text = "회원가입 실패";
        }
    }
    #endregion


    void initText()
    {
        _inputId.text = null;
        _inputPW.text = null;
        _inputMBTI.text = null;
    }

    bool CheckMBTI(string _mbti)
    {
        string mbti = _mbti.ToUpper();

        if (mbti == null || string.IsNullOrEmpty(mbti) || mbti.Length != 4)
            return false;

        if (mbti[0] != 'I' && mbti[0] != 'E')
            return false;

        if (mbti[1] != 'S' && mbti[1] != 'N')
            return false;

        if (mbti[2] != 'T' && mbti[2] != 'F')
            return false;

        if (mbti[3] != 'J' && mbti[3] != 'P')
            return false;

        return true;
    }
  
}