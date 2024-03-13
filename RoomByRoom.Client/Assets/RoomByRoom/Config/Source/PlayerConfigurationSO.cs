using Configuration;
using UnityEngine;
using Zenject;

namespace RoomByRoom.Control
{
  [CreateAssetMenu(fileName = CreateAssetMenuNames.PLAYER_CONFIGURATION_FILE_NAME, menuName = CreateAssetMenuNames.PLAYER_CONFIGURATION_MENU_NAME)]
  public class PlayerConfigurationSO : ScriptableObject, ISelfBinder<PlayerConfigurationSO>
  {
    public PlayerConfiguration Value;
    public void Bind(DiContainer container)
    {
      (this as ISelfBinder<PlayerConfigurationSO>).BindSelf(container);
    }
  }
}