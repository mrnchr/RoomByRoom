using RoomByRoom.UI.Game.HUD;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
  public class UnitView : View
  {
    [HideInInspector] public Animator Anim;
    [HideInInspector] public AttackCatcher AttackCtr;
    [HideInInspector] public Rigidbody Rb;
    public Bar HealthBar;
    public Bar ArmorBar;

    protected virtual void Awake()
    {
      Rb = GetComponent<Rigidbody>();
      Anim = GetComponent<Animator>();
      AttackCtr = GetComponent<AttackCatcher>();
    }

    public virtual void StartAttackAnimation()
    {
    }
  }
}