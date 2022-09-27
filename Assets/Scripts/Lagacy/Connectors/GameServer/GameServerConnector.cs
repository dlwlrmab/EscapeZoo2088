using CommonProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Connectors.GameServer
{
    public sealed class GameServerConnector : Singleton<GameServerConnector>
    {
        public GameServerConnectorHelper Helper 
            = new GameServerConnectorHelper(TargetConfiguration.Targets.WebServer);

        private readonly ListQueue<GameServerConnectorHelper.PostData> _listQueue = new ListQueue<GameServerConnectorHelper.PostData>();

        private bool _isRequest;
        private bool _isInit;
        private Coroutine _waitCoroutine;

        void Update()
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
            if (_isRequest || _listQueue.Count == 0)
                return;

            Debug.Log("BBBBBBBBBBBBBBBBB");
            OnRequest();
        }

        public void RequestToWebServer(MessageType messageType, byte[] request, Action<byte[]> onCompleted, bool isLoading = true)
        {
            _listQueue.Enqueue(new GameServerConnectorHelper.PostData(messageType, request, onCompleted, isLoading));
        }

        private void OnRequest()
        {
            _isRequest = true;
            var temp = _listQueue.Dequeue();

            if (temp.IsLoading)
            {
                //FrontUIManager.Instance.PlayLoading();
            }

            StopWaitCoroutine();

            _waitCoroutine = StartCoroutine(CoWait());
            Helper.Post(new GameServerConnectorHelper.PostData(temp.MessageType, temp.Protocol, delegate (byte[] response)
            {
                OnResponse();
                temp.OnComplted(response);
            }));
        }

        private IEnumerator CoWait()
        {
            var deltaTime = 0f;

            while (deltaTime < 2)
            {
                deltaTime += Time.smoothDeltaTime;
                yield return null;
            }

            //FrontUIManager.Instance.SetActiveLoadingIndicator(true);

            while (deltaTime < 5)
            {
                deltaTime += Time.smoothDeltaTime;
                yield return null;
            }

            //FrontUIManager.Instance.OpenNetworkNotReachable();
        }

        private void OnResponse()
        {
            if (_listQueue.Count == 0)
            {
                //FrontUIManager.Instance.StopLoading();
            }

            StopWaitCoroutine();
            _isRequest = false;
        }

        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
            Helper.OnWebSessionError += OnWebExceptionCallback;
        }

        private void OnWebExceptionCallback()
        {
            _isRequest = false;
            StopWaitCoroutine();
        }

        public void StopWaitCoroutine()
        {
            if (_waitCoroutine != null)
                StopCoroutine(_waitCoroutine);
        }
    }
}
