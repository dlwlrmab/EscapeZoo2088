using CommonProtocol;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;


public class LoginScene : MonoBehaviour
{
    [SerializeField] Toggle _toggleGogame;

    [Header("Login Popup")]
    [SerializeField] Text _textLoginPopupNotice;
    [SerializeField] Text _inputLoginPopupId;
    [SerializeField] Text _inputLoginPopupPW;

    [Header("Join Popup")]
    [SerializeField] Text _textJoinPopupNotice;
    [SerializeField] Text _inputJoinPopupId;
    [SerializeField] Text _inputJoinPopupPW;
    [SerializeField] Text _inputJoinPopupMBTI;

    SceneLoadManager _scenemanager = null;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();

#if UNITY_EDITOR
        _toggleGogame.gameObject.SetActive(true);
        GlobalData.isGogame = PlayerPrefs.GetInt("saveGogame") == 1;
        _toggleGogame.isOn = GlobalData.isGogame;

#else
        _toggleGogame.gameObject.SetActive(false);

#endif
    }

    #region OnClick

    public void OnClickLoginPopup()
    {
#if UNITY_EDITOR
        string saveId = PlayerPrefs.GetString("saveId");
        string savePw = PlayerPrefs.GetString("savePw");

        if (GlobalData.isGogame && !string.IsNullOrEmpty(saveId) && !string.IsNullOrEmpty(savePw))
        {
            ReqLogin(saveId, savePw);
            return;
        }
#endif

        _textLoginPopupNotice.text = "아이디와 비밀번호를 입력해 주세요.";

        _inputLoginPopupId.text = null;
        _inputLoginPopupPW.text = null;
    }

    public void OnClickGoLogin()
    {
        string id = _inputLoginPopupId.text;
        string pw = _inputLoginPopupPW.text;

        if (id == string.Empty || id == null)
        {
            _textLoginPopupNotice.text = "ID를 입력하지 않았습니다.";
            return;
        }
        if (pw == string.Empty || pw == null)
        {
            _textLoginPopupNotice.text = "PW를 입력하지 않았습니다.";
            return;
        }

        ReqLogin(id, pw);
    }

    public void OnClickJoinPopup()
    {
        _textJoinPopupNotice.text = "아이디, 비밀번호, MBTI를 입력해 주세요.";
        _inputJoinPopupId.text = null;
        _inputJoinPopupPW.text = null;
        _inputJoinPopupMBTI.text = null;
    }

    public void OnClickGoJoin()
    {
        string id = _inputJoinPopupId.text;
        string pw = _inputJoinPopupPW.text;
        string mbti = _inputJoinPopupMBTI.text;

        if (id == string.Empty || id == null)
        {
            _textJoinPopupNotice.text = "ID를 입력하지 않았습니다.";
            return;
        }
        if (pw == string.Empty || pw == null)
        {
            _textJoinPopupNotice.text = "PW를 입력하지 않았습니다.";
            return;
        }
        if (!CheckMBTI(mbti))
        {
            _textJoinPopupNotice.text = "잘못된 MBTI 입니다.";
            return;
        }

        ReqJoinAccount(id, pw, mbti);
    }

    public void ToggleGogame()
    {
        GlobalData.isGogame = _toggleGogame.isOn;
        PlayerPrefs.SetInt("saveGogame", GlobalData.isGogame ? 1 : 0);
    }

    #endregion

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

    

    #region reqServer

    void ReqLogin(string id, string pw)
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
            
            PlayerPrefs.SetString("saveId", id);
            PlayerPrefs.SetString("savePw", pw);
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

    public void RecvLoginResult(string responseString)
    {
        var res = JsonConvert.DeserializeObject<ResLogin>(responseString);

        if (res.ResponseType == ResponseType.Fail)
        {
            _textLoginPopupNotice.text = "아이디 또는 비밀번호를 확인해 주세요.";
            return;
        }
        else
        {
            _textLoginPopupNotice.text = "로그인 성공!";

            GlobalData.id = res.userId;
            GlobalData.mbti = _inputLoginPopupPW.text.ToUpper();
            _scenemanager.PlayFadeout(null, "LobbyScene");
        }
    }

    public void RecvJoinResult(string responseBytes)
    {
        var res = JsonConvert.DeserializeObject<ResAccountJoin>(responseBytes);

        if (res.ResponseType == ResponseType.DuplicateName)
            _textJoinPopupNotice.text = "이미 존재하는 ID입니다.";
        else if (res.ResponseType == ResponseType.Success)
            _textJoinPopupNotice.text = "회원가입 성공!";
        else
            _textJoinPopupNotice.text = "회원가입 실패";
    }

    #endregion
}