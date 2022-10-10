using Assets.Scripts;
using CommonProtocol;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using MessagePack;
using BattleProtocol;
using EnumDef;


public class LobbyScene : MonoBehaviour
{
    [SerializeField] GameObject _playButton;
    [SerializeField] GameObject _readyButton;
    [SerializeField] PopupMyInfo _popupMyInfo;
    [SerializeField] GameObject _popupNoti;
    [SerializeField] Text _popupNotiText;

    SceneLoadManager _scenemanager = null;
    GameObject _exMapButton = null;
    GameObject _exAnimalButton = null;
    bool _serverTimeOut = false;
    bool _stopmatching = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_stopmatching)
        {
            _stopmatching = true;
        }
    }

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
        GlobalData.map = MAP.NONE;
        GlobalData.myAnimal = ANIMAL.NONE;

        if (GlobalData.isGogame)
        {
            GlobalData.map = MAP.DESERT;
            GlobalData.myAnimal = ANIMAL.CHICKEN;

            PlayGame();
        }
    }

    #region reqServer

    // 서버로 준비메시지를 보냄(같은팀 인원모으기)
    void ReqTryingMatch()
    {
        var req = new ReqTryMatch
        {
            userId = GlobalData.myId,
            character = (int)GlobalData.myAnimal,
            gameMap = (int)GlobalData.map,
            score = GlobalData.myScore,
            MessageType = CommonProtocol.MessageType.TryMatching,
        };

        ResTryMatch res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "MatchRequest", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResTryMatch>(responseString);
            if(res.ResponseType != ResponseType.Success || string.IsNullOrEmpty(res.ticketId))
            {
                ShowNotiPopup("티켓요청에 실패했습니다.");
            }
            else
            {
                CheckMatchStatus(res.ticketId);
            }
            
        }));
    }

    void CheckMatchStatus(string ticketId)
    {
        var req = new ReqMatchStatus();
        req.ticketIds.Add(ticketId);

        string jsondata = JsonConvert.SerializeObject(req);

        StartCoroutine(CoWaitOtherUser(jsondata));
    }

    // 매칭될떄까지 상태체크 반복
    IEnumerator CoWaitOtherUser(string jsondata)
    {
        ShowNotiPopup("매칭 시작");
        ResMatchStatus res = null;
        bool success = false;

        ShowNotiPopup(Strings.WaitOtherUser, false);
        SceneLoadManager.Instance.SetLoading(true);

        while (true)
        {
            StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "MatchStatus", (responseString) =>
            {
                res = JsonConvert.DeserializeObject<ResMatchStatus>(responseString);
                // 팀원이 다차면 ResponseType.Success 
                if (res.ResponseType == ResponseType.Success)
                    success = true;
            }, false));

            if (success)
                break;

            if(_stopmatching)
                break;

            yield return new WaitForSeconds(0.3f);
        }

        SceneLoadManager.Instance.SetLoading(false);
        if (!_stopmatching)
        {
            if(res != null && res.ResponseType == ResponseType.Success)
            {
                ShowNotiPopup("매칭 성공");
                ConnectBattleServer(res);
                _readyButton.gameObject.SetActive(false);
                _playButton.gameObject.SetActive(true);
            }
            else
            {
                ShowNotiPopup(Strings.ServerError);
            }   
        }
        else
        {
            ShowNotiPopup("매칭을 취소하였습니다.");
            _stopmatching = false; 
        }
        
    }
    
    public void ConnectBattleServer(ResMatchStatus res)
    {
        if (GameManager.Instance.IsTryMatching)
            return;

        GlobalData.GameSessionId = res.GameSessionId;
        GlobalData.PlayerSessionId = res.PlayerSessionId;
        GlobalData.Port = res.Port;
        GlobalData.myTeamName = res.TeamName;
        GlobalData.roundList = res.roundList;

        GameManager.Instance.IsTryMatching = true;

        BattleServerConnector.Instance.Connect(res.IpAddress, res.Port, "0");

        while (false == BattleServerConnector.Instance.IsConnected) ;
        Invoke("ServerTimeOut", 3f);
        while (false == BattleServerConnector.Instance.IsConnected && !_serverTimeOut) ;

        if (_serverTimeOut)
        {
            ResConnectBattleServer(false);
            return;
        }

        CancelInvoke("ServerTimeOut");
        _serverTimeOut = false;

        ShowNotiPopup("배틀서버 연결 성공!");

        BattleServerConnector.Instance.Send(BattleProtocol.MessageType.BattleEnter,
                MessagePackSerializer.Serialize(new ProtoBattleEnter
                {
                    Msg = BattleProtocol.MessageType.BattleEnter,
                    UserId = GameManager.Instance.UserId,
                    GameSessionId = res.GameSessionId,
                    PlayerSessionId = res.PlayerSessionId,
                }));

        ResConnectBattleServer(true);
    }

    // 배틀서버연결에 무한정 기다리는 경우가있어, 3초 타임아웃
    void ServerTimeOut()
    {
        _serverTimeOut = true;
    }

    public void ReqMyInfo()
    {
        var req = new ReqMyPage
        {
            userId = GlobalData.myId,
        };

        string jsondata = JsonConvert.SerializeObject(req);

        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "myPage", (responseString) => {
            ResMyPageData(responseString);
        }));

    }

    #endregion

    #region resServer

    // 배틀서버 연결
    public void ResConnectBattleServer(bool success)
    {
        if (success)
        {
            ShowNotiPopup("매치 메이킹 성공, 게임을 시작합니다.");
            Invoke("PlayGame", 1f);
            return;

            ShowNotiPopup(Strings.WaitOtherUser);

            _readyButton.SetActive(false);

            // 서버연결이후 제거
            _playButton.SetActive(true);

            // 이때 배경에서 캐릭터 움직이기가능.
            // 매칭된 사람의 캐릭터도 나와야함.
            // 다른사람이 매칭된지 어떻게 알지?
            // 게임 플레이하려면 어떻게 하는지..?
            // 플레이어 타입 수정 필요할듯
            // 메시지타입도 추가필요할듯
        }
        else
        {
            ShowNotiPopup("배틀서버 타임아웃");
            _readyButton.SetActive(true);
        }
    }

    public void ResMyPageData(string responseString)
    {
        _popupMyInfo.gameObject.SetActive(true);
        ResMyPage res = JsonConvert.DeserializeObject<ResMyPage>(responseString);
        if (res != null && res.ResponseType == ResponseType.Success)
        {
            _popupMyInfo.SetData(res, true);
        }
        else
        {
            _popupMyInfo.SetData(null, false);
        }
    }

    #endregion

    #region buttonClick
    public void ChoiceMap(GameObject obj)
    {
        if (_exMapButton != null)
        {
            _exMapButton.GetComponent<Image>().color = Color.white;
        }
        _exMapButton = obj;
        GlobalData.map = (EnumDef.MAP)int.Parse(obj.name);
        obj.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    public void ChoiceAnimal(GameObject obj)
    {
        if (_exAnimalButton != null)
        {
            _exAnimalButton.GetComponent<Image>().color = Color.white;
        }
        _exAnimalButton = obj;
        GlobalData.myAnimal = (ANIMAL)int.Parse(obj.name);
        obj.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    public void OnClickReadyGame()
    {
        if (GlobalData.map == MAP.NONE)
        {
            ShowNotiPopup("맵을 선택하지 않았습니다.");
            return;
        }
        if (GlobalData.myAnimal == ANIMAL.NONE)
        {
            ShowNotiPopup("캐릭터를 선택하지 않았습니다.");
            return;
        }

        // 서버 작업완료이후 수정되어야할 코드들
        ReqTryingMatch();
    }


    public void OnClickMyInfo()
    {
        if (!_popupMyInfo.gameObject.activeSelf)
            ReqMyInfo();
    }


    #endregion

    #region popup

    void ShowNotiPopup(string msg, bool close = true)
    {
        CancelInvoke("InvokeClosePopupNoti");
        _popupNoti.SetActive(true);
        _popupNotiText.text = msg;
        Debug.LogAssertion(msg);

        if (close)
            Invoke("InvokeClosePopupNoti", 3f);
    }

    void InvokeClosePopupNoti()
    {
        _popupNoti.SetActive(false);
    }
    #endregion

    // 플레이어튼 사용하지않고 매칭 성공시 자동시작
    public void PlayGame()
    {
        _scenemanager.PlayFadeout(null, "IngameScene");
    }

    #region 구현필요?

    public void OnClickExit()
    {
        // 서버와의 연결을 끊고, 로그인씬으로 이동
        // 서버연결끊는 로직 필요
        _scenemanager.PlayFadeout(null, "LoginScene");
    }

    #region otherPlayer
    // 다른유저가 로비에서 나간경우
    public void ExitUser()
    {
        // 해당플레이어 프리팹 제거
        // 유저리스트에서 제거

        if (GlobalData.playerInfos.Count < 5)
        {
            _playButton.SetActive(false);
        }
    }

    // 다른유저가 로비에 참여한 경우
    public void joinUser()
    {
        // 해당플레이어 프리팹 생성
        // 유저리스트에 추가
        if (GlobalData.playerInfos.Count == 5)
        {
            _playButton.SetActive(true);
        }
    }
    #endregion
    #endregion
}