using UnityEngine;

namespace RoomByRoom
{
    [CreateAssetMenu(menuName = "RoomByRoom/Data/Configuration")]
    public class Configuration : ScriptableObject
    {
        public Vector2 MouseSensitivity;
        public string DefaultSaveName;
        public bool IsNewGame;
        public bool SaveInFile;
    }
}