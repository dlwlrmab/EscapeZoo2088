namespace CommonProtocol
{
    public class ReqAccountJoin : CBaseProtocol
    {
        public string userId;
        public string password;
        public string mbti;
    }

    public class ReqLogin : CBaseProtocol
    {
        public string userId;
        public string password;
    }
}
