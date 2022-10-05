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
        string id = _inputId.text;

        if (id == string.Empty || id == null)
        {
            _notiText.text = "ID를 입력하지 않았습니다.";
            return;
        }

        // 서버 작업 이후 수정되어야할 코드들
        Login(id);
    }

    public void OnClickJoinButton()
    {
        string id = _inputId.text;

        if (id == string.Empty || id == null)
        {
            _notiText.text = "ID를 입력하지 않았습니다.";
            return;
        }

        // 서버 작업 이후 수정되어야할 코드들
        JoinAccount(id);
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
    void Login(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        var req = new ReqLogin
        {
            userId = id,
            password = "1234",
        };

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        var responseBytes
            = webClient.UploadString(new Uri("https://opupgoihqd.execute-api.ap-northeast-2.amazonaws.com/test/") + "Login", "POST"
            , JsonConvert.SerializeObject(req));

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
            RecvLoginResult(true);
        }
    }

    void JoinAccount(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        var req = new ReqAccountJoin
        {
            userId = id,
            password = "1234",
            mbti = "infj",
        };

        int i = 0;
        if (i == 0)
        {
            string json = JsonUtility.ToJson(req);
            StartCoroutine(RequestPost("https://opupgoihqd.execute-api.ap-northeast-2.amazonaws.com/test/AccountJoin/", json));
        }
        else
        {
            var webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            var responseBytes
            = webClient.UploadString(new Uri("https://opupgoihqd.execute-api.ap-northeast-2.amazonaws.com/test/") + "AccountJoin", "POST"
            , JsonConvert.SerializeObject(req));

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
        }
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