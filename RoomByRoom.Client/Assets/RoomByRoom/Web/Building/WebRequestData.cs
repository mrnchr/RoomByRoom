using System;
using UnityEngine.Networking;

namespace RoomByRoom.Web.Building
{
    public class WebRequestData
    {
        public bool HasData { get; set; }
        public string Data { get; set; }
        public string Method { get; set; }
        public string LocalUrl { get; set; }
        public Action<UnityWebRequest> Configurator { get; set; }
        public bool IsAuthorized;
    }
}