using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class AttackAnimationCatcher : MonoBehaviour, IEcsInitSystem
    {
        public HumanoidView OwnView;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void OnStartAttackAnimation()
        {
            // OwnView.MainWeapon.SetActiveAttackTriggers(true);
            // _world.GetPool<Damaging>().Add(OwnView.MainWeapon.Entity);
        }

        public void OnStopAttackAnimation()
        {
            // OwnView.MainWeapon.SetActiveAttackTriggers(false);
            // _world.GetPool<Damaging>().Del(OwnView.MainWeapon.Entity);
        }

        public void OnReset()
        {
            TryGetComponent<HumanoidView>(out OwnView);
        }
    }
}