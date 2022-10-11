using System.Collections.Generic;

﻿namespace CommonProtocol
{
    public class ResAccountJoin : CBaseProtocol
    {

    }

    public class ResLogin : CBaseProtocol
    {
        public int score;
    }

    public class ResMyPage : CBaseProtocol
    {
        public int winCnt;
        public int lossCnt;
        public int score;
        public string mbti;
    }

    public class ResRanking : CBaseProtocol
    {
        public List<string> teamNameList = new List<string>();
        public List<List<string>> memberList = new List<List<string>>();
        public List<int> scoreList = new List<int>();
    }

    public class ResTryMatch : CBaseProtocol
    {
        public string ticketId;
    }

    public class ResMatchStatus : CBaseProtocol
    {
        public string IpAddress;
        public string PlayerSessionId;
        public string GameSessionId; 
        public int Port;
        public string TeamName;
        public List<int> roundList;
    }
}