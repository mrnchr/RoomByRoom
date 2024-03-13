using RoomByRoom;
using UnityEngine;
using Zenject;

namespace Configuration
{
    [CreateAssetMenu(fileName = CreateAssetMenuNames.PROFILE_FILE_NAME, menuName = CreateAssetMenuNames.PROFILE_MENU_NAME)]
    public class ProfileConfig : ScriptableObject, ISelfBinder<ProfileConfig>
    {
        public Sprite DefaultAvatar;
        
        public void Bind(DiContainer container)
        {
            (this as ISelfBinder<ProfileConfig>).BindSelf(container);
        }
    }
}