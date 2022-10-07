using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static string id = string.Empty;
    public static string mbti = string.Empty;

    // 서버연결될떄까지 선택한 캐릭터 종류 임시저장
    public static int mapIndex = -1;
    public static int animalIndex = -1;

    public static string GameSessionId = string.Empty;
    public static string TargetServer = "Dev";

    public static bool isHost = true;  // 한 그룹내에서 여러명이 게임을 시작할수도 있어, 호스트 지정이 필요하거나, 인원수가 차면 자동으로 시작하거나 하는 규칙이 필요할듯?
    public static List<Player> playerList = new List<Player>(); // 함께 플레이하는 유저들의 데이터 저장

    public static string GatewayAPI = "https://opupgoihqd.execute-api.ap-northeast-2.amazonaws.com/test/";
}