using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
  public class GroundUnitView : UnitView
  {
    public LayerMask GroundMask;
    protected static readonly int _isRunning = Animator.StringToHash("IsRunning");
    protected static readonly int _isJumping = Animator.StringToHash("IsFlying");

    public virtual void AnimateRun(bool run) => Anim.SetBool(_isRunning, run);
    public virtual void AnimateJump(bool jump) => Anim.SetBool(_isJumping, jump);
  }
}