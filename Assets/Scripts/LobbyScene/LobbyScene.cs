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

public class LobbyScene : MonoBehaviour
{
    [SerializeField] Text _notiText;
    [SerializeField] GameObject _playButton;
    [SerializeField] GameObject _readyButton;

    SceneLoadManager _scenemanager = null;
    GameObject _exMapButton = null;
    GameObject _exAnimalButton = null;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
        GlobalData.mapIndex = -1;
        GlobalData.animalIndex = -1;
    }

    
    #region reqServer
    // 서버로 준비메시지를 보냄(같은팀 인원모으기)
    void SendReqTryingMatch()
    {
        var infos = ConfigReader.Instance.GetInfos<Infos>();

        var req = new CommonProtocol.ReqTryingMatch
        {
            userId = GlobalData.id,
            mapIndex = GlobalData.mapIndex,
            animalIndex = GlobalData.animalIndex,
            MessageType = CommonProtocol.MessageType.TryMatching,
        };

        var webClient = new WebClient();
        ResTryMatch res = null;

        int i = 1;
        // 둘다 사용가능한 것 같고, 서버 구현에따라 선택하면 될듯
        // i == 0 일반 프로토콜? 사용하는형식
        // else 람다함수 사용하는 방식
        if (i == 0)
        {
            var message = MessagePackSerializer.Serialize(req);
            SendProtocolManager.Instance.SendProtocolReq(message, req.MessageType.ToString(), (responseBytes) =>
            {
                res = MessagePackSerializer.Deserialize<ResTryMatch>(responseBytes);
            });
        }
        else
        {
            string jsondata = JsonConvert.SerializeObject(req);
            SendProtocolManager.Instance.SendLambdaReq(jsondata, "TryMatching" ,(responseString) => {
                res = JsonConvert.DeserializeObject<ResTryMatch>(responseString);
            });
            
        }

        if (res != null && res.ResponseType == ResponseType.Success)
        {
           //  ????? gamesessionid, playersessionid?
           // ReqOwnTeamMember(res.IpAddress, res.Port, res.GameSessionId);
        }
    }

    // 매치메이킹(팀원매칭)
    private void ReqOwnTeamMember(string battleServerIp, int battleServerPot, string gameSessionId, string playerSessionId)
    {
        if (GameManager.Instance.IsTryMatching)
            return;

        GameManager.Instance.IsTryMatching = true;

        BattleServerConnector.Instance.Connect(battleServerIp, battleServerPot, "0");

        while (false == BattleServerConnector.Instance.IsConnected) ;

        ResOwnTeamMember(true);

        GlobalData.GameSessionId = gameSessionId;
        BattleServerConnector.Instance.Send(BattleProtocol.MessageType.BattleEnter,
                MessagePackSerializer.Serialize(new ProtoBattleEnter
                {
                    Msg = BattleProtocol.MessageType.BattleEnter,
                    UserId = GameManager.Instance.UserId,
                    GameSessionId = gameSessionId,
                    PlayerSessionId = playerSessionId,
                }));
    }

    // 서버로 매치메이킹 요청 보냄(다른팀 매칭)
    public void ReqMakeMatchMaking()
    {
        
    }

    public void ReqMyPageData()
    {
        var req = new CommonProtocol.ReqMyPage
        {
            userId = GlobalData.id,
        };

        string jsondata = JsonConvert.SerializeObject(req);

        // type 서버와 상의하여 정해야함 (임시 : "MyPage")
        SendProtocolManager.Instance.SendLambdaReq(jsondata, "MyPage", (responseString) => {
            ResMyPageData(responseString);
        });
    }

    #endregion

    #region resServer
    // 서버로부터 준비에대한 응답을 받음
    // 본인이 호스트가 아닌 경우 (성공여부, 이미 준비하고있었던 유저 리스트)
    // 본인이 호스트인경우 (성공여부)
    public void ResOwnTeamMember(bool success)
    {
        if (success)
        {
            _notiText.text = "다른유저를 기다리고있습니다.";
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
            _notiText.text = "서버연결에 실패하였습니다. 재시도해주세요";
            _readyButton.SetActive(true);
        }
    }

    public void ResMyPageData(string responseString)
    {
        ResMyPage res = JsonConvert.DeserializeObject<ResMyPage>(responseString);
        //if (res != null && res.ResponseType == ResponseType.Success)
        {
            // 성공 처리
        }
        //else
        {
            // 실패처리
        }
    }

    // 서버로부터 매치메이킹 결과 받음
    public void RecvMakeMatchMakingResult(bool success)
    {
        if (success)
        {
            _notiText.text = "매치 메이킹 성공";
            _scenemanager.PlayFadeout(null, "IngameScene");
        }
        else
        {
            _notiText.text = "매치 메이킹 실패 재시도 해주세요";
            _playButton.SetActive(true);
        }
    }

    #endregion

    #region otherPlayer
    // 다른유저가 로비에서 나간경우
    public void ExitUser()
    {
        // 해당플레이어 프리팹 제거
        // 유저리스트에서 제거

        if (GlobalData.playerList.Count < 5)
        {
            _playButton.SetActive(false);
        }
    }

    // 다른유저가 로비에 참여한 경우
    public void joinUser()
    {
        // 해당플레이어 프리팹 생성
        // 유저리스트에 추가
        if (GlobalData.playerList.Count == 5)
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
        GlobalData.mapIndex = int.Parse(obj.name);
        obj.GetComponent<Image>().color = Color.green;
    }

    public void ChoiceAnimal(GameObject obj)
    {
        if (_exAnimalButton != null)
        {
            _exAnimalButton.GetComponent<Image>().color = Color.white;
        }
        _exAnimalButton = obj;
        GlobalData.animalIndex = int.Parse(obj.name);
        obj.GetComponent<Image>().color = Color.green;
    }

    public void OnClickReadyGame()
    {
        if (GlobalData.mapIndex == -1)
        {
            _notiText.text = "맵을 선택하지 않았습니다.";
            return;
        }
        if (GlobalData.animalIndex == -1)
        {
            _notiText.text = "캐릭터를 선택하지 않았습니다.";
            return;
        }

        // 서버 작업완료이후 수정되어야할 코드들
        SendReqTryingMatch();
    }

    public void OnClickPlayGame()
    {
        if (!GlobalData.isHost)
        {
            _notiText.text = "호스트만 게임을 시작할수있습니다.";
            return;
        }

        // 서버 작업완료 이후 수정되어야할 코드들
        //MakeMatchMaking();
        RecvMakeMatchMakingResult(true);
    }

    public void OnClickMyPage()
    {

    }
    public void OnClickExit()
    {
        // 서버와의 연결을 끊고, 로그인씬으로 이동
        // 서버연결끊는 로직 필요
        _scenemanager.PlayFadeout(null, "LoginScene");
    }
    #endregion


}