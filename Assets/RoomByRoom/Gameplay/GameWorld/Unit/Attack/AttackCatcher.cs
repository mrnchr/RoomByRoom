using UnityEngine;

namespace RoomByRoom
{
    public class AttackCatcher : MonoBehaviour
    {
        protected UnitView _ownView;
        protected AttackService _attackSvc;

        protected void Awake()
        {
            _ownView = GetComponent<UnitView>();
        }

        public void SetService(AttackService attackSvc)
        {
            _attackSvc = attackSvc;
        }

        public void OnStartAttackAnimation()
        {
            _attackSvc.SetAttackTriggers(_ownView.Entity, true);
        }

        public void OnStopAttackAnimation()
        {
            _attackSvc.SetAttackTriggers(_ownView.Entity, false);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger && other.TryGetComponent<UnitView>(out UnitView unit))
            {
                _attackSvc.Damage(_ownView.Entity, unit.Entity);
            }
        }
    }
}