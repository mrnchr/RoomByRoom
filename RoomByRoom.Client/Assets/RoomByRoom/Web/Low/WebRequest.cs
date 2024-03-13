using Configuration;
using UnityEngine;
using UnityEngine.Networking;

namespace RoomByRoom.Web.Low
{
    public class WebRequest : IWebRequest
    {
        private readonly WebConfig _config;

        public WebRequest(WebConfig config)
        {
            _config = config;
        }

        public UnityWebRequest Create(string method, string localUrl, string requestData)
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requestData);

            UnityWebRequest www = Create(method, localUrl);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);

            return www;
        }

        public UnityWebRequest Create(string method, string localUrl)
        {
            var www = new UnityWebRequest
            {
                method = method,
                url = _config.ServerName + localUrl,
                downloadHandler = new DownloadHandlerBuffer()
            };
            
            www.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            return www;
        }
    }
}