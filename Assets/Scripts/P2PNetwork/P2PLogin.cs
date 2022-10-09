using EuNet.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using EnumDef;

public class P2PLogin : MonoBehaviour
{
    [SerializeField] private Button _gameStartBtn;

    private void Start()
    {
        _gameStartBtn.OnClickAsAsyncEnumerable().Subscribe(PushGameStartBtn);
    }

    public async UniTaskVoid PushGameStartBtn(AsyncUnit asyncUnit)
    {
        if (GlobalData.map == MAP.NONE)
        {//맵을 선택하지 않았습니다.
            return;
        }
        if (GlobalData.myAnimal == ANIMAL.NONE)
        {//캐릭터를 선택하지 않았습니다
            return;
        }

        try
        {
            _gameStartBtn.interactable = false;

            var client = NetClientGlobal.Instance.Client;

            var result = await client.ConnectAsync(GlobalData.Port ,TimeSpan.FromSeconds(10));

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
