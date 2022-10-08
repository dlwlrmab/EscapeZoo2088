﻿using Newtonsoft.Json;
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
    public static string GameSessionId = string.Empty;
    public static string TargetServer = "Dev";

    public static string id = string.Empty;
    public static string mbti = string.Empty;

    // 로비
    public static bool isHost = true;  // 한 그룹내에서 여러명이 게임을 시작할수도 있어, 호스트 지정이 필요하거나, 인원수가 차면 자동으로 시작하거나 하는 규칙이 필요할듯?
    public static ANIMAL animal = ANIMAL.NONE;

    // 인게임: 로비에서 인게임 진입 시 서버에게 받을 데이터들
    public static MAP map = MAP.NONE;
    public static int[] roundList = null;
    public static int roundIndex = -1;
    public static int roundMax = -1;
    public static List<PlayerInfo> playerInfos = null;

    // 인게임: 진행 중 받을 데이터들
    public static int enemyRoundIndex = 1;
}

public class PlayerInfo
{
    public string Id;
    public string MBTI;
    public ANIMAL Animal;
}

public class Strings : MonoBehaviour
{
    public static string MypageFail = "데이터 로드 실패";
}