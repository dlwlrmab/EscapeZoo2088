using BattleProtocol;
using MessagePack;

// 람다로 서버에 요청을 보내는경우 json 을 보내기떄문에 직렬화할필요없음 (application/json)

// 일반 프로토콜로 요청을 보내는경우 직렬화 하여 보내야하기때문에, (application/octet-stream)
// [MessagePackObject] 선언후 각 변수마다 key 값 지정필요

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

    public class ReqMyPage : CBaseProtocol
    {
        public string userId;
    }

    [MessagePackObject]
    public class ReqTryingMatch
    {
        [Key(0)]
        public MessageType MessageType;
        [Key(1)]
        public string userId;
        [Key(2)]
        public int mapIndex;
        [Key(3)]
        public int animalIndex;
    }

    [MessagePackObject]
    public class ProtoBattleEnter : BaseProtocol
    {
        [Key(1)]
        public string GameSessionId;
        [Key(2)]
        public string PlayerSessionId;
        [Key(3)]
        public int UserId;
    }
}
