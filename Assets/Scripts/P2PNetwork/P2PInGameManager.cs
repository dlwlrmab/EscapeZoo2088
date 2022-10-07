
using Cysharp.Threading.Tasks;
using EuNet.Core;
using EuNet.Unity;
using UnityEngine;

[ExecutionOrder(-10)]
[RequireComponent(typeof(NetView))]
public class P2PInGameManager : SceneSingleton<P2PInGameManager>, INetViewHandler
{
    public Actor ControlActor { get; set; }
    private NetView _view;

    protected override void Awake()
    {
        base.Awake();

        _view = GetComponent<NetView>();
    }

    private async UniTaskVoid Start()
    {
        if (NetClientGlobal.Instance.MasterIsMine() == false)
        {
            // 마스터가 아니므로 마스터로부터 현재 게임 상황을 받아서 복구시킴
            await RecoveryAsync();
        }
    }

    public async UniTask<bool> RecoveryAsync()
    {
        await UniTask.DelayFrame(1);
        await NetClientGlobal.Instance.RequestRecovery();
        return true;
    }

    public GameObject CreateMyPlayer()
    {
        GameObject playerObj = null;
        NetPool.DataWriterPool.Use((writer) =>
        {
            // 내가 주인인 플레이어를 생성함
            playerObj = NetClientGlobal.Instance.Instantiate(
                "Prefabs/Player/PlayerMe",
                new Vector3(Random.Range(-0.1f, 0.1f), 0.5f, 0f),
                Quaternion.identity,
                writer);

            // 컨트롤을 할 수 있도록 등록
            ControlActor = playerObj.GetComponent<Actor>();
        });
        return playerObj;
    }

    public void OnViewInstantiate(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }

    public void OnViewDestroy(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }

    public void OnViewMessage(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }
}
