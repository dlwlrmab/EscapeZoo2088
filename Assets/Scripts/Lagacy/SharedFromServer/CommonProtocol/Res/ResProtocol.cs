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
        public string teamName;
    }

    public class ResMatchResult : CBaseProtocol
    {
        public int score;
    }
}