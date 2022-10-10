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

    public static string GameSessionId = string.Empty;
    public static string PlayerSessionId = string.Empty;
    public static int Port = 0;

    public static bool isGogame = false;

    // 내 정보
    public static string myId = string.Empty;
    public static string myMBTI = string.Empty;
    public static ANIMAL myAnimal = ANIMAL.NONE;
    public static int myScore = 0;
    public static string myTeamName = string.Empty;

    // 인게임
    public static MAP map = MAP.NONE;
    public static List<PlayerInfo> playerInfos = null;
    public static List<int> roundList = null;
    public static int roundIndex = -1;
    public static int enemyRoundIndex = 0;
    public static bool isWinner = false;
    public static int sunriseTime = 0;
    public static int roundMax = 2;  // 클라 결정 : 총 라운드 수
    public static int teamUserCount = 2; // 클라 결정 : 팀당 인원 수
}

public class PlayerInfo
{
    public string Id;
    public string MBTI;
    public int animal;
}

public class Strings : MonoBehaviour
{
    public static string MypageFail = "데이터 로드 실패";
    public static string ServerError = "서버연결에 실패하였습니다. 재시도해주세요.";
    public static string WaitOtherUser = "다른유저를 기다리고 있습니다... ESC로 취소할 수 있습니다.";
}