using UnityEngine;
using Zenject;

namespace Configuration
{
    public interface IConfigProvider
    {
        public TConfig Get<TConfig>() where TConfig : ScriptableObject;
        public void BindConfigs(DiContainer container);
    }
}