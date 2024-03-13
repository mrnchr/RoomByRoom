using UnityEngine;

namespace Infrastructure.Logging
{
    public class PersonalLogger : IPersonalLogger
    {
        public void Log(object message, string group = "", LogType type = LogType.Log)
        {
            Debug.unityLogger.Log(type, $"{group}: {message}");
        }

        public void Log(object message, Object context, string group = "", LogType type = LogType.Log)
        {
            Debug.unityLogger.Log(type, message: $"{group}: {message}", context);
        }
    }
}