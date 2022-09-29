using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //ReadyGame();
        RecvReadyGame(true);

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

    // 서버로 준비메시지를 보냄(같은팀 인원모으기)
    void ReadyGame()
    {

    }
    // 서버로부터 준비에대한 응답을 받음
    // 본인이 호스트가 아닌 경우 (성공여부, 이미 준비하고있었던 유저 리스트)
    // 본인이 호스트인경우 (성공여부)
    public void RecvReadyGame(bool success)
    {
        if (success)
        {
            _notiText.text = "다른유저를 기다리고있습니다.";
            _readyButton.SetActive(false);

            // 서버연결이후 제거
            _playButton.SetActive(true);
        }
        else
        {
            _notiText.text = "서버연결에 실패하였습니다. 재시도해주세요";
            _readyButton.SetActive(true);
        }
    }

    // 서버로 매치메이킹 요청 보냄
    public void MakeMatchMaking()
    {

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

    public void OnClickExit()
    {
        // 서버와의 연결을 끊고, 로그인씬으로 이동
        // 서버연결끊는 로직 필요
        _scenemanager.PlayFadeout(null, "LoginScene");
    }

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
}