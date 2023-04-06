using UnityEngine;
using UnityEngine.AI;

namespace RoomByRoom
{
    public class HumanoidMovement : MonoBehaviour
    {
        private Transform _player;
        private NavMeshPath _path;
        private HumanoidView _humanoid;

        void Start()
        {
            _humanoid = GetComponent<HumanoidView>();
            _player = FindObjectOfType<PlayerView>().transform;
            _path = new NavMeshPath();
            Debug.Log(_path.corners.Length);

        }

        void Update()
        {
            NavMesh.CalculatePath(transform.position, _player.position, NavMesh.AllAreas, _path);
            Vector3 target = _path.corners[1];
            Vector3 dir = target - transform.position;
            // Debug.Log($"target: {target}, dir: {dir}, pos: {transform.position}, aim: {target - transform.position}");
            dir.y = 0;
            dir = dir.normalized;
            _humanoid.Rb.velocity = dir * _humanoid.MovingCmp.Speed;
        }
    }
}
