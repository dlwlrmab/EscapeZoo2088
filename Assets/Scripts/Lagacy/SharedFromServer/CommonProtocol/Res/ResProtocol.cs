﻿namespace CommonProtocol
{
    public class ResAccountJoin : CBaseProtocol
    {
        public string userId;
    }

    public class ResLogin : CBaseProtocol
    {
        public string userId;
    }

    public class ResMyPage : CBaseProtocol
    {
        public int winCnt;
        public int lossCnt;
        public int score;
        public string mbti;
        public string userId;
    }

    public class ResTryMatch : CBaseProtocol
    {
        public string IpAddress; // battleServerIp
        public int Port; // battleServerPot
        public string GameSessionId; // gameSessionId
        public string ticketId;
        //public string IpAddress; // battleServerIp
        //public int Port; // battleServerPot
        //public string GameSessionId; // gameSessionId
    }

    public class ResMatchStatus : CBaseProtocol
    {
        public string IpAddress;
        public string PlayerSessionId;
        public int Port;
    }
}