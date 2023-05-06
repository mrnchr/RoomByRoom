using UnityEngine;

namespace RoomByRoom
{
  public class AttackCatcher : MonoBehaviour
  {
    private AttackService _attackSvc;
    private UnitView _ownView;

    protected void Awake()
    {
      _ownView = GetComponent<UnitView>();
    }

    // must catch damaged unit
    public void OnTriggerEnter(Collider other)
    {
      if (other.isTrigger && other.TryGetComponent(out WeaponView weapon))
        _attackSvc.Damage(_ownView.Entity, weapon.Entity);
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
  }
}