//To use Messagepack in Unity, 
//you should refer to the link below. 
//After installing mpc and creating MessagePackGenerated, 
//you can use it by calling related functions when executing the process.
//https://github.com/neuecc/MessagePack-CSharp/tree/v2.1.90#aot-code-generation-to-support-unityxamarin
//https://velog.io/@1111/%EA%B2%8C%EC%9E%84-%EC%84%9C%EB%B2%84-%EA%B0%9C%EB%B0%9C-%EA%B4%80%EB%A0%A8-%EA%B0%84%EB%8B%A8-%EB%A9%94%EB%AA%A8
//mpc -i "./Assembly -CSharp.csproj" -o "./Assets/Scripts/MessagePackGenerated.cs"
//MessagePack 2.0.323

using BattleProtocol.Entities;
using CommonType;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public sealed class GameManager : Singleton<GameManager>
    {
        public  string TargetServer;
        public int UserId;
        public string Name;
        public string GameSessionId;
        public PlayerType PlayerType;

        public GameObject Knight;
        public GameObject KnightPrefab;
        public GameObject EnemyKnight;
        public GameObject EnemyKnightPrefab;
        public GameObject LoginCanvas;
        public GameObject LoginCanvasPrefab;
        public GameObject LobbyCanvas;
        public GameObject LobbyCanvasPrefab;
        public GameObject NoticeCanvas;
        public GameObject BattleEndCanvas;
        public GameObject BattleEndCanvasPrefab;
        public GameObject BattlePlayingCanvas;
        public GameObject BattleControllerCanvasPrefab;
        public GameObject RoomListsCanvasPrefab;
        public GameObject RoomListsCanvas;
        public bool IsTryMatching = false;

        private PlayerType _playerType;

        public void Start()
        {
            StaticCompositeResolver.Instance.Register(new IFormatterResolver[] {
                MessagePack.Resolvers.GeneratedResolver.Instance,
                MessagePack.Resolvers.StandardResolver.Instance,
            });

            var resolver = StaticCompositeResolver.Instance;
            MessagePackSerializer.DefaultOptions = 
                MessagePackSerializerOptions.Standard.WithResolver(resolver);

            //Knight = Instantiate(KnightPrefab, transform);
            //EnemyKnight = Instantiate(EnemyKnightPrefab, transform);
            //LoginCanvas = Instantiate(LoginCanvasPrefab, transform);
            //LobbyCanvas = Instantiate(LobbyCanvasPrefab, transform);
            //BattleEndCanvas = Instantiate(BattleEndCanvasPrefab, transform);
            //BattlePlayingCanvas = Instantiate(BattleControllerCanvasPrefab, transform);
            //RoomListsCanvas = Instantiate(RoomListsCanvasPrefab, transform);

            //Knight.SetActive(true);
            //EnemyKnight.SetActive(false);
            //LoginCanvas.SetActive(true);
            //LobbyCanvas.SetActive(false);
            //NoticeCanvas.SetActive(false);
            //BattleEndCanvas.SetActive(false);
            //BattlePlayingCanvas.SetActive(false);
            //RoomListsCanvas.SetActive(false);
        }

        private void Notice(string text)
        {
            NoticeCanvas.GetComponentInChildren<Text>().text = text;
            NoticeCanvas.SetActive(true);
        }

        public void TryMatching()
        {
            if (BattleServerConnector.Instance.IsConnected)
            {
                //LoginCanvas.SetActive(false);
                Notice("상대를 찾는 중...");
            }
        }

        public void MatchingSuccess(PlayerType playerType, long gameStartDateTimeTicks, int gameSecond)
        {
            _playerType = playerType;
            NoticeCanvas.SetActive(false);
            LobbyCanvas.SetActive(false);
            RoomListsCanvas.SetActive(false);
            BattlePlayingCanvas.SetActive(true);
            GameManager.Instance.BattlePlayingCanvas.GetComponent<BattlePlaying>().BattleStart(gameStartDateTimeTicks, gameSecond);
            EnemyKnight.SetActive(true);

            //GameManager.Instance.Knight.GetComponent<KnightControl>().idle();
        }

        public void BattleEnd(ProtoBattleResult proto)
        {
            if (proto.IsBattleEnd)
            {
                BattleServerConnector.Instance.Close();
                BattleEndCanvas.SetActive(true);
                if (proto.Winner == _playerType)
                    Voctory();
                else if (proto.Winner == PlayerType.None)
                    Drow();
                else
                    Loss();
            }
            else
            {

                if (proto.Winner == _playerType)
                    GetScore();
                else if (proto.Winner == PlayerType.None)
                    Drow();
                else
                    TakeDamage();

                GameManager.Instance.BattlePlayingCanvas.GetComponent<BattlePlaying>().RoundStart(proto.RoundStartDateTimeTicks, proto.RoundSecond);
                //GameManager.Instance.Knight.GetComponent<KnightControl>().idle(1);
                //GameManager.Instance.EnemyKnight.GetComponent<KnightControl>().idle(1);
            }
        }

        private void Voctory()
        {
          //  Knight.GetComponent<KnightControl>().skill_2(false);
            //EnemyKnight.GetComponent<KnightControl>().death(false);
            EnemyKnight.GetComponent<Knight>().TakeDamage(1);
            BattleEndCanvas.GetComponentInChildren<Text>().text = "승리!!!";
        }

        private void Loss()
        {
            Knight.GetComponent<Knight>().TakeDamage(1);
            //EnemyKnight.GetComponent<KnightControl>().skill_2(false);
            //Knight.GetComponent<KnightControl>().death(false);
            BattleEndCanvas.GetComponentInChildren<Text>().text = "패배!!!";
        }

        private void Drow()
        {
            //Knight.GetComponent<KnightControl>().getHit(false);
            //EnemyKnight.GetComponent<KnightControl>().getHit(false);
            BattleEndCanvas.GetComponentInChildren<Text>().text = "무승부!!!";
        }

        private void GetScore()
        {
            //Knight.GetComponent<KnightControl>().skill_1(false);
            //EnemyKnight.GetComponent<KnightControl>().getHit(false);
            EnemyKnight.GetComponent<Knight>().TakeDamage(1);
            BattleEndCanvas.GetComponentInChildren<Text>().text = "";
        }

        private void TakeDamage()
        {
            //Knight.GetComponent<KnightControl>().getHit(false);
            Knight.GetComponent<Knight>().TakeDamage(1);
            //EnemyKnight.GetComponent<KnightControl>().skill_1(false);
            BattleEndCanvas.GetComponentInChildren<Text>().text = "";
        }

        public void WriteTxt(string filePath, string message)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            FileStream fileStream
                = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

            writer.WriteLine(message);
            writer.Close();
        }
    }
}
