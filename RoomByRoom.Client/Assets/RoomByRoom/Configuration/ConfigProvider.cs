using System.Collections.Generic;
using System.Linq;
using RoomByRoom;
using UnityEngine;
using Zenject;

namespace Configuration
{
    [CreateAssetMenu(fileName = CreateAssetMenuNames.PROVIDER_FILE_NAME, menuName = CreateAssetMenuNames.PROVIDER_MENU_NAME)]
    public class ConfigProvider : ScriptableObject, IConfigProvider
    {
        [SerializeField] private List<ScriptableObject> _configs;

        public TConfig Get<TConfig>() where TConfig : ScriptableObject
        {
            return _configs.OfType<TConfig>().FirstOrDefault();
        }

        public void BindConfigs(DiContainer container)
        {
            foreach (ISelfBinder binder in _configs.OfType<ISelfBinder>())
            {
                binder.Bind(container);
            }
        }
    }
}