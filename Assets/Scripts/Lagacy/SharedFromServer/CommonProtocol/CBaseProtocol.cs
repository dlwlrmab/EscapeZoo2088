using UnityEngine;
using System.Collections.Generic;

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
        public ResponseType ResponseType; // res 만
        public string gameSessionId;
        public string playerSessionId;

        public string userId;
        public string mbti;
        public int animal;
        public int score; // res 만

        public string teamName;
        public int teamUserCount;

        public int preRoundNum;
        public int endRoundNum;
        public int currentRoundNum;
        public int enemyRoundNum; // res 만

        public List<PlayerInfos> playerInfos = null; // res 만
        public List<int> roundList = new List<int>(); // startround res.

        public int sunriseTime; // res 만
        public bool isWinner;

        public IngameProcotol()
        {
            gameSessionId = GlobalData.GameSessionId;
            playerSessionId = GlobalData.PlayerSessionId;

            userId = GlobalData.myId;
            mbti = GlobalData.myMBTI;
            animal = (int)GlobalData.myAnimal;

            teamName = GlobalData.myTeamName;
            teamUserCount = GlobalData.teamUserCount;

            preRoundNum = GlobalData.roundIndex;
            endRoundNum = GlobalData.roundMax;
            currentRoundNum = GlobalData.roundIndex + 1;
            isWinner = GlobalData.isWinner;
        }
    }
}