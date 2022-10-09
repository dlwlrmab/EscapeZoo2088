using UnityEngine;

namespace CommonProtocol
{
    public class CBaseProtocol
    {
        public MessageType MessageType;
        public ResponseType ResponseType;
        public string userId;
    }

    public class IngameProcotol
    {
        public MessageType MessageType;
        public ResponseType ResponseType;
        public string userId;

        public string gameSessionId;
        public string playerSessionId;
        public string teamName;

        public IngameProcotol()
        {
            if (!string.IsNullOrEmpty(GlobalData.myId))
                userId = GlobalData.myId;
            else
                Debug.LogAssertion("BaseProtocol userid is null");

            if (!string.IsNullOrEmpty(GlobalData.GameSessionId))
                gameSessionId = GlobalData.GameSessionId;
            else
                Debug.LogAssertion("BaseProtocol GameSessionId is null");

            if (!string.IsNullOrEmpty(GlobalData.myTeamName))
                teamName = GlobalData.myTeamName;
            else
                Debug.LogAssertion("BaseProtocol TeamName is null");

            if (!string.IsNullOrEmpty(GlobalData.PlayerSessionId))
                playerSessionId = GlobalData.PlayerSessionId;
            else
                Debug.LogAssertion("BaseProtocol PlayerSessionId is null");
        }

    }
}
