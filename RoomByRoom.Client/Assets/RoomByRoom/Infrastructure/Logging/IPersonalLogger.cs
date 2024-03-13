using UnityEngine;

namespace Infrastructure.Logging
{
    public interface IPersonalLogger
    {
        public void Log(object message, string group = "", LogType type = LogType.Log);
        public void Log(object message, Object context, string group = "", LogType type = LogType.Log);
    }
}