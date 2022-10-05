namespace CommonProtocol
{
    public class ResAccountJoin : CBaseProtocol
    {
        public ResponseType ResponseType;
        public string userId;
    }

    public class ResLogin : CBaseProtocol
    {
        public ResponseType ResponseType;
        public string userId;
    }
}
