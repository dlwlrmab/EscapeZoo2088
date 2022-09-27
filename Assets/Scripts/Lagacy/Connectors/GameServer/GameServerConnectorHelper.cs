using CommonProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


public class GameServerConnectorHelper : IDisposable
{
    public struct PostData
    {
        public MessageType MessageType;
        public byte[] Protocol;
        public bool IsLoading;
        public Action<byte[]> OnComplted;

        public PostData(MessageType messageType, byte[] protocol, Action<byte[]> onComplted, bool isLoading = true)
        {
            MessageType = messageType;
            Protocol = protocol;
            OnComplted = onComplted;
            IsLoading = isLoading;
        }
    }

    public GameServerConnectorHelper(string webBaseUri)
    {
        _webBaseUri = webBaseUri;
    }

    private readonly string _headerStream = "application/octet-stream";
    private readonly string _post = "POST";

    public Action OnWebSessionError;

    private string _webBaseUri;
    private PostData _postData;

    public void Post(PostData postData)
    {
        _postData = postData;

        using (var webClient = new WebClient())
        {
            webClient.Headers[HttpRequestHeader.ContentType] = _headerStream;

            try
            {
                webClient.UploadDataCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                        OnException(e.Error);
                    else
                        _postData.OnComplted?.Invoke(e.Result);
                };
                //webClient.UploadDataAsync(new Uri(GetAddress(_postData.MessageType)), _post, _postData.Protocol);
                webClient.UploadData(new Uri(GetAddress(_postData.MessageType)), _post, _postData.Protocol);
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }
    }

    private string GetAddress(MessageType messageType)
    {
        return new Uri(_webBaseUri) + messageType.ToString();
    }

    private void OnException(Exception e)
    {
        OnWebSessionError?.Invoke();
    }

    public void Dispose()
    {
    }
}

