using UnityEngine;
using UnityEngine.AI;

namespace RoomByRoom
{
  public class HumanoidMovement : MonoBehaviour
  {
    private NavMeshAgent _agent;
    private HumanoidView _humanoid;
    private NavMeshPath _path;
    private Transform _player;

    private void Start()
    {
      _humanoid = GetComponent<HumanoidView>();
      _player = FindObjectOfType<PlayerView>().transform;
      _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
      // if ((transform.position - _player.transform.position).sqrMagnitude > 3f)
      _agent.SetDestination(_player.transform.position);
    }
  }
}