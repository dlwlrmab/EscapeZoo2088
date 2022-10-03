using EuNet.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class P2PLogin : MonoBehaviour
{
    [SerializeField] private Button _gameStartBtn;

    private void Start()
    {
        _gameStartBtn.OnClickAsAsyncEnumerable().Subscribe(PushGameStartBtn);
    }

    public async UniTaskVoid PushGameStartBtn(AsyncUnit asyncUnit)
    {
        try
        {
            _gameStartBtn.interactable = false;

            var client = NetClientGlobal.Instance.Client;

            var result = await client.ConnectAsync(TimeSpan.FromSeconds(10));

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
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            if (_gameStartBtn != null)
                _gameStartBtn.interactable = true;
        }
    }
}
