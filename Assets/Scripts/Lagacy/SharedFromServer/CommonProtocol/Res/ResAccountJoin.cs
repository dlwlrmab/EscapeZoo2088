namespace CommonProtocol
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
    }

    public class ResTryMatch : CBaseProtocol
    {
        public string IpAddress;
        public int Port;
        public string GameSessionId;
    }
}
