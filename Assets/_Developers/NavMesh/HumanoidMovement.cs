using UnityEngine;
using UnityEngine.AI;

namespace RoomByRoom
{
    public class HumanoidMovement : MonoBehaviour
    {
        private Transform _player;
        private NavMeshPath _path;
        private HumanoidView _humanoid;
        private NavMeshAgent _agent;

        void Start()
        {
            _humanoid = GetComponent<HumanoidView>();
            _player = FindObjectOfType<PlayerView>().transform;
            _agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            // if ((transform.position - _player.transform.position).sqrMagnitude > 3f)
                _agent.SetDestination(_player.transform.position);
        }
    }
}
