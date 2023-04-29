using UnityEngine;
using UnityEngine.AI;

namespace RoomByRoom
{
	public class Climbing : MonoBehaviour
	{
		[SerializeField] protected Transform _lowerStepChecker;
		[SerializeField] protected Transform _upperStepChecker;
		[SerializeField] protected float _stepHeight;
		[SerializeField] protected float _stepSmooth;
		[SerializeField] protected float _stepDistance;
		[SerializeField] protected float _footLength;
		protected Rigidbody _rb;

		private void Start()
		{
			Vector3 checkerPos = _lowerStepChecker.position;
			checkerPos.y += _stepHeight;
			_upperStepChecker.position = checkerPos;

			_rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			// Debug.DrawLine(_lowerStepChecker.position, _lowerStepChecker.position + transform.forward * _stepDistance);
			// Debug.DrawLine(_upperStepChecker.position,
			//     _upperStepChecker.position + transform.forward * (_stepDistance + _footLength));
			StepChecker();
		}

		protected virtual void StepChecker()
		{
			if (!Physics.Raycast(_lowerStepChecker.position, transform.forward, out RaycastHit lowerHit,
				    _stepDistance)) return;
			if (Physics.Raycast(_upperStepChecker.position, transform.forward, _stepDistance + _footLength)) return;
			Vector3 initialPos = lowerHit.point + transform.forward * _footLength;
			initialPos.y = _upperStepChecker.position.y;

			if (!Physics.Raycast(initialPos, Vector3.down, out RaycastHit hit)) return;
			if (!Physics.Raycast(hit.point, -transform.forward, out RaycastHit backHit)) return;
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