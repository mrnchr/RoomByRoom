using System.Collections;
using System.Threading.Tasks;
using Infrastructure.Logging;
using Newtonsoft.Json;
using RoomByRoom.Web.Building;
using UnityEngine.Networking;

namespace RoomByRoom.Web.Sender
{
    public class WebRequestSender : IWebRequestSender
    {
        private readonly WebRequestData _data;
        private readonly IWebRequestBuilder _builder;
        private readonly IPersonalLogger _logger;
        private RequestSnapshot _snapshot;

        public WebRequestSender(WebRequestData data, IWebRequestBuilder builder, IPersonalLogger logger)
        {
            _data = data;
            _builder = builder;
            _logger = logger;
        }

        public RequestSnapshot Snapshot => _snapshot;

        public TResponseData GetData<TResponseData>()
        {
            return Snapshot.GetData<TResponseData>();
        }

        public bool IsConnectionError()
        {
            return _snapshot != null && _snapshot.Result == UnityWebRequest.Result.ConnectionError;
        }

        public bool IsSuccess()
        {
            return _snapshot != null && _snapshot.Result == UnityWebRequest.Result.Success;
        }

        public IEnumerator SendRequest(bool freeAfter = false)
        {
            using UnityWebRequest request = _builder.Build(_data);
            {
                LogRequest(request);
                yield return request.SendWebRequest();

                CopyRequestData(request);
            }
        }

        public async Task SendRequestAsync(bool freeAfter = false)
        {
            using UnityWebRequest request = _builder.Build(_data);
            {
                LogRequest(request);
                UnityWebRequestAsyncOperation operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();
                
                CopyRequestData(request);
            }
        }

        private void CopyRequestData(UnityWebRequest request)
        {
            _snapshot = new RequestSnapshot
            {
                RawData = request.downloadHandler.text,
                Result = request.result,
                IsDone = request.isDone
            };
        }

        private void LogRequest(UnityWebRequest request)
        {
            _logger.Log($"[{request.method}] {request.url}", LoggingGroups.WEB);
        }
    }

    public class RequestSnapshot
    {
        public string RawData;
        public UnityWebRequest.Result Result;
        public bool IsDone;

        public TData GetData<TData>()
        {
            return JsonConvert.DeserializeObject<TData>(RawData);
        }
    }
}