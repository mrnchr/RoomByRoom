using RoomByRoom;
using UnityEngine;
using Zenject;

namespace Configuration
{
    [CreateAssetMenu(fileName = CreateAssetMenuNames.HUB_FILE_NAME, menuName = CreateAssetMenuNames.HUB_MENU_NAME)]
    public class HubConfig : ScriptableObject, ISelfBinder<HubConfig>
    {
        public Color DefaultUserColor;
        public Color GroupAuthorColor;
        
        public Color DefaultGroupColor;
        public Color LoadingGroupColor;
        
        public void Bind(DiContainer container)
        {
            (this as ISelfBinder<HubConfig>).BindSelf(container);
        }
    }
}