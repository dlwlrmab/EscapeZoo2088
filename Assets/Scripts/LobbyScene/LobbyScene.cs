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

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
        GlobalData.map = MAP.NONE;
        GlobalData.animal = ANIMAL.NONE;
    }


    #region reqServer
    // 서버로 준비메시지를 보냄(같은팀 인원모으기)
    void ReqTryingMatch()
    {
        var req = new ReqTryMatch
        {
            userId = GlobalData.id,
            //animalIndex = (int)GlobalData.animal,
            gameMap = (int)GlobalData.map,
            MessageType = CommonProtocol.MessageType.TryMatching,
        };

        var webClient = new WebClient();
        ResTryMatch res = null;

        string jsondata = JsonConvert.SerializeObject(req);
        StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "MatchRequest", (responseString) =>
        {
            res = JsonConvert.DeserializeObject<ResTryMatch>(responseString);
            CheckMatchStatus(res.ticketId);
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

        while (true)
        {
            StartCoroutine(SendProtocolManager.Instance.CoSendLambdaReq(jsondata, "MatchStatus", (responseString) =>
            {
                res = JsonConvert.DeserializeObject<ResMatchStatus>(responseString);
                // 팀원이 다차면 ResponseType.Success 
                if (res.ResponseType == ResponseType.Success)
                    success = true;
            }));

            if (success)
                break;

            yield return new WaitForSeconds(1);
        }

        ShowNotiPopup("매칭 성공");
        ConnectBattleServer(res);
    }
    
    private void ConnectBattleServer(ResMatchStatus res)
    {
        if (res != null && res.ResponseType == ResponseType.Success)
        {
            if (GameManager.Instance.IsTryMatching)
                return;

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

            //GlobalData.GameSessionId = res.GameSessionId;
            BattleServerConnector.Instance.Send(BattleProtocol.MessageType.BattleEnter,
                    MessagePackSerializer.Serialize(new ProtoBattleEnter
                    {
                        Msg = BattleProtocol.MessageType.BattleEnter,
                        UserId = GameManager.Instance.UserId,
                        //GameSessionId = gameSessionId,
                        PlayerSessionId = res.PlayerSessionId,
                    }));

            ResConnectBattleServer(true);
        }
        else
        {
            ShowNotiPopup(Strings.ServerError);
        }
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
            userId = GlobalData.id,
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

    // 서버로부터 매치메이킹 결과 받음
    public void RecvMakeMatchMakingResult(bool success)
    {
        if (success)
        {
            _scenemanager.PlayFadeout(null, "IngameScene");
        }
        else
        {
            _playButton.SetActive(true);
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
        GlobalData.animal = (ANIMAL)int.Parse(obj.name);
        obj.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    }

    public void OnClickReadyGame()
    {
        PlayGame();

        if (GlobalData.map == MAP.NONE)
        {
            ShowNotiPopup("맵을 선택하지 않았습니다.");
            return;
        }
        if (GlobalData.animal == ANIMAL.NONE)
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

    #region 사용안하는듯..?

    public void OnClickPlayGame()
    {
        if (!GlobalData.isHost)
        {
            return;
        }

        // 서버 작업완료 이후 수정되어야할 코드들
        //MakeMatchMaking();
        RecvMakeMatchMakingResult(true);
    }
    
    // 서버로 매치메이킹 요청 보냄(다른팀 매칭)
    public void ReqMakeMatchMaking()
    {
        // 서버와의 연결을 끊고, 로그인씬으로 이동
        // 서버연결끊는 로직 필요
        _scenemanager.PlayFadeout(null, "LoginScene");

    }
    #endregion


}