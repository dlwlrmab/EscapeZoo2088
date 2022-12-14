using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using CommonProtocol;
using MessagePack;
using EnumDef;


public class GlobalData : MonoBehaviour
{
    public static string GatewayAPI = "https://opupgoihqd.execute-api.ap-northeast-2.amazonaws.com/test/";
    public static string TargetServer = "Dev";

    public static string GameSessionId = "ksh-001";
    public static string PlayerSessionId = string.Empty;
    public static int Port = 12000;

    public static bool isGogame = false;

    // 내 정보
    public static string myId = string.Empty;
    public static string myMBTI = string.Empty;
    public static ANIMAL myAnimal = ANIMAL.NONE;
    public static int myScore = 0;
    public static string myTeamName = "blue";

    // 인게임
    public static MAP map = MAP.NONE;
    public static List<PlayerInfos> playerInfos = new List<PlayerInfos>();
    public static List<int> roundList = new List<int>() { 0, 1, 2, 3 };
    public static int roundIndex = 0;
    public static int roundRetryCount = 0;
    public static int enemyRoundIndex = 1;
    public static bool isWinner = false;
    public static int sunriseTime = 0;
    public static int roundMax = 4;  // 클라 결정 : 총 라운드 수
    public static int teamUserCount = 2; // 클라 결정 : 팀당 인원 수
    public static int teamUserMaxCount = 5; // 고정된 값이며 참조용
    public static int playingUserCount = 0; // 현재 같이 게임진행하는 유저수 , reqexitgame에 현재 플레이어수가 필요, RecvEnterGame 에서 초기화
}

public class PlayerInfos
{
    public string userId;
    public string mbti;
    public int animal;
}

public class Strings : MonoBehaviour
{
    public static string MypageFail = "데이터 로드 실패";
    public static string ServerError = "서버연결에 실패하였습니다. 재시도해주세요.";
    public static string WaitOtherUser = "다른유저를 기다리고 있습니다... ESC로 취소할 수 있습니다.";
}