using RoomByRoom;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Configuration
{
    [CreateAssetMenu(fileName = CreateAssetMenuNames.SCENE_FILE_NAME, menuName = CreateAssetMenuNames.SCENE_MENU_NAME)]
    public class SceneConfig : ScriptableObject, ISelfBinder<SceneConfig>
    {
        public string LoadingScene;
        public string SignScene;
        public string MainScene;
        [FormerlySerializedAs("SessionScene")] public string GameScene;

        public void Bind(DiContainer container)
        {
            (this as ISelfBinder<SceneConfig>).BindSelf(container);
        }
    }
}