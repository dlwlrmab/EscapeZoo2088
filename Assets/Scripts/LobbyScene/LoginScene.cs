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
        JoinAccount(id, pw, mbti);
    }

    // 서버로부터 로그인 결과를 받아서 처리
    // 필요한 데이터 
    // 로그인 성공여부, 플레이가능한 라운드?
    public void RecvLoginResult(bool success)
    {
        if (success)
            _notiText.text = "로그인 성공";
        else
        {
            _notiText.text = "로그인 실패";
            return;
        }

        GlobalData.id = _inputId.text;
        _loginButton.gameObject.SetActive(false);
        _scenemanager.PlayFadeout(null, "LobbyScene");
    }

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
        
        var webClient = new WebClient();
        string jsondata = JsonConvert.SerializeObject(req);

        Debug.Log($"LoginData : {jsondata}");

        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        var responseBytes
            = webClient.UploadString(new Uri(GlobalData.GatewayAPI) + "Login", "POST", jsondata);

        //var webClient = new WebClient();
        //webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
        //var responseBytes
        //    = webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
        //    , MessagePackSerializer.Serialize(req));

        //var res = MessagePackSerializer.Deserialize<ResAccountJoin>(responseBytes);

        var res = JsonConvert.DeserializeObject<ResLogin>(responseBytes);

        if (res.ResponseType == ResponseType.Fail)
        {
            _notiText.text = "존재하지 않는 ID입니다.";
            RecvLoginResult(false);
        }
        else
        {
            GlobalData.id = res.userId;
            initText();
            RecvLoginResult(true);
        }
    }

    void JoinAccount(string id, string pw, string mbti)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        var req = new ReqAccountJoin
        {
            userId = id,
            password = pw,
            mbti = mbti,
        };

        int i = 1;
        if (i == 0)
        {
            string json = JsonUtility.ToJson(req);
            StartCoroutine(RequestPost(GlobalData.GatewayAPI, json));
        }
        else
        {
            var webClient = new WebClient();

            string jsondata = JsonConvert.SerializeObject(req);

            Debug.Log($"JoinData : {jsondata}");

            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            var responseBytes
            = webClient.UploadString(new Uri(GlobalData.GatewayAPI) + "AccountJoin", "POST", jsondata);

            var res = JsonConvert.DeserializeObject<ResAccountJoin>(responseBytes);

            //var webClient = new WebClient();
            //webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
            //var infos = ConfigReader.Instance.GetInfos<Infos>();
            //var responseBytes
            //= webClient.UploadData(new Uri(infos.GameServer.Address) + req.MessageType.ToString(), "POST"
            //, MessagePackSerializer.Serialize(req));

            //var res = MessagePackSerializer.Deserialize<ResAccountJoin>(responseBytes);

            if (res.ResponseType == ResponseType.DuplicateName)
            {
                _notiText.text = "이미 존재하는 ID입니다.";
            }
            else if(res.ResponseType == ResponseType.Success)
            {
                _notiText.text = "회원가입 성공, 로그인 해주세요";
                initText();
            }
            else
            {
                _notiText.text = "회원가입 실패";
            }
        }
    }

    void initText()
    {
        _inputId.text = null;
        _inputPW.text = null;
        _inputMBTI.text = null;
    }

    bool CheckMBTI(string _mbti)
    {
        string mbti = _mbti.ToLower();

        if (mbti == null || string.IsNullOrEmpty(mbti) || mbti.Length != 4)
            return false;

        if (mbti[0] != 'i' && mbti[0] != 'e')
            return false;

        if (mbti[1] != 's' && mbti[1] != 'n')
            return false;

        if (mbti[2] != 't' && mbti[2] != 'f')
            return false;

        if (mbti[3] != 'j' && mbti[3] != 'p')
            return false;

        return true;
    }
    // 서버연결 테스트용 함수
    // 연결 완료후 삭제
    IEnumerator RequestPost(string URL, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // 네트워크 에러 체크
            if (request.isNetworkError)
            {
                Debug.Log("네트워크 에러");
                yield break;
            }

            var text = request.downloadHandler.text;

            if (text != null)
                Debug.Log(text);

            // 받은 데이터 JSON으로 변환
            //JsonData = data = null;
            //try
            //{
            //    data = Json.Parse(request.downloadHandler.text);
            //}
            //catch (Exception)
            //{
            //    // 예외처리
            //    yield break;
            //}

            //if (data == null)
            //{
            //    // json 데이터가 빈 상황 예외처리
            //    yield break;
            //}
        }

    }
}