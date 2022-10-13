using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumDef;
using EuNet.Unity;
using Common;

public class IngameScene : MonoBehaviour
{
    public static IngameScene _instance = null;

    public static IngameScene Instance
    {
        get
        {
            if (_instance == null)
            {
                System.Type tType = typeof(IngameScene);
                _instance = FindObjectOfType(tType) as IngameScene;

                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<IngameScene>();
                    _instance.gameObject.name = tType.Name;
                }
            }
            return _instance;
        }
    }

    [Header("Controller")]
    [SerializeField] IngamePacketHandler _packetHandler;
    [SerializeField] IngameMapController _mapController;
    [SerializeField] IngamePlayerController _playerController;
    [SerializeField] IngameUIController _uiController;
    [SerializeField] IngameLoadingController _loadingController;
    [SerializeField] IngameEndingController _endingController;

    public IngamePacketHandler PacketHandler { get { return _packetHandler; } }
    public IngameMapController MapController { get { return _mapController; } }
    public IngamePlayerController PlayerController { get { return _playerController; } }

    SceneLoadManager _scenemanager = null;

    public INGAME_STATE State { get; private set; } = INGAME_STATE.LOADING;

    private void Start()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
        ConnectP2PServer();

        if (IngamePacketHandler.isTest)
        {
            GlobalData.roundList = new List<int>() { 0, 1, 2, 3 };
            GlobalData.roundIndex = -1;
            GlobalData.roundMax = GlobalData.roundList.Count;
        }

        InitGame();
    }

    #region Init

    private void InitGame()
    {
        State = INGAME_STATE.LOADING;
        _packetHandler.SendEnterGame();
        _loadingController.LoadStartLoading();
    }

    public void EnterGame()
    {
        _mapController.CreateMapAndRound();
        _playerController.CreatePlayer();
    }

    public void CompleteStartLoading()
    {
        StartCoroutine(WaitInitGame());
    }

    private IEnumerator WaitInitGame()
    {
        while (true)
        {
            if (_mapController.CreateComplete && _playerController.CreateComplete)
                break;
            yield return null;
        }

        _packetHandler.SendStartRound();
    }

    #endregion

    public void LoadRound()
    {
        State = INGAME_STATE.PLAYING;
        _mapController.LoadRound(); // 라운드 셋팅 먼저
        _playerController.LoadRound();
        _uiController.LoadRound();
        _loadingController.HideStartLoading();
        _loadingController.LoadRoundLoading();
    }

    public void CompleteRoundLoading()
    {
        StartRound();
    }

    public void StartRound()
    {
        _mapController.StartRound();
        _playerController.StartRound();
        _uiController.StartRound();
    }

    public void ReStartRound(Player deadPlayer)
    {
        _playerController.PlayerResetPreStep();
        _uiController.ShowDeadAnimal(deadPlayer.GetAnimal());

        if (deadPlayer.IsMine)
            IngameScene.Instance.PacketHandler.SendRestartRound();
    }

    public void ClearGame()
    {
        State = INGAME_STATE.ENDING;
        _playerController.ClearGame();
        _uiController.ClearGame();
        _endingController.LoadEnding();
    }

    public async void ConnectP2PServer()
    {
        var client = NetClientGlobal.Instance.Client;
        // port: 서버 매칭 구현시 GlobalData.port 적용하도록
        var result = await client.ConnectAsync(12000, System.TimeSpan.FromSeconds(10));
        if (result == true)
        {
            LoginRpc loginRpc = new LoginRpc(client);
            var loginResult = await loginRpc.Login(SystemInfo.deviceUniqueIdentifier);

            Debug.Log($"Login Result : {loginResult}");
            if (loginResult != 0)
                return;

            var joinResult = await loginRpc.Join();
            Debug.Log($"Join : {joinResult}");
        }
        else
        {
            Debug.LogError("Fail to connect server");
        }
    }

    public void DisConnectP2PServer()
    {
        // disconnect 콜백
        NetClientGlobal.Instance.Client.OnClosed = () =>
        {
            Debug.LogWarning("P2P Server Disconnect");

            StopAllCoroutines();
            State = INGAME_STATE.LOADING;
            _scenemanager.PlayFadeout(null, "LobbyScene");
        };

        NetClientGlobal.Instance.Client.Disconnect(); // p2p 서버 disconnect   
    }
}