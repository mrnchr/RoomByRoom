using UnityEngine;

namespace RoomByRoom
{
  public class PlayerClimbing : Climbing
  {
    [SerializeField] private Transform _character;

    protected override void StepChecker()
    {
      if (!Physics.Raycast(_lowerStepChecker.position, _character.forward, out RaycastHit lowerHit,
                           _stepDistance)) return;
      if (Physics.Raycast(_upperStepChecker.position, _character.forward, _stepDistance + _footLength)) return;
      Vector3 initialPos = lowerHit.point + _character.forward * _footLength;
      initialPos.y = _upperStepChecker.position.y;

      if (!Physics.Raycast(initialPos, Vector3.down, out RaycastHit hit)) return;
      if (!Physics.Raycast(hit.point, -_character.forward, out RaycastHit backHit)) return;
      if (backHit.rigidbody == _rb)
      {
        Vector3 pos = transform.position;
        pos.y = hit.point.y + 0.1f;
        _rb.position = pos;
      }
      else
      {
        _rb.MovePosition(transform.position + Vector3.up * _stepSmooth);
      }
    }
  }
}