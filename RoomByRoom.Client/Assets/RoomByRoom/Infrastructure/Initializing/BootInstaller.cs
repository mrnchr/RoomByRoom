using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private EntryPoint _entryPoint;
        
        public override void InstallBindings()
        {
            BindEntryPoint();
        }

        private void BindEntryPoint()
        {
            Container
                .BindInterfacesTo<EntryPoint>()
                .FromInstance(_entryPoint)
                .AsSingle();
        }
    }
}