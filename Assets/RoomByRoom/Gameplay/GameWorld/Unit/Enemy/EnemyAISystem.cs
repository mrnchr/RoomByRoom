using UnityEngine;
using UnityEngine.AI;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class EnemyAISystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<ControllerByAI, UnitViewRef>> _enemies = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            int playerEntity = world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
            Transform player = world.GetComponent<UnitViewRef>(playerEntity).Value.transform;

            foreach (int index in _enemies.Value)
            {
                if(world.GetComponent<UnitInfo>(index).Type != UnitType.Humanoid)
                    continue;

                HumanoidView humanoid = (HumanoidView)world.GetComponent<UnitViewRef>(index).Value;
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(humanoid.transform.position, player.position, NavMesh.AllAreas, path);
                foreach(var cor in path.corners)
                {
                    Debug.Log(cor);
                }
                Debug.Log($"---Entity {index}");
                Vector3 target = path.corners[1];
                Vector3 dir = target - humanoid.transform.position;
                // Debug.Log($"target: {target}, dir: {dir}, pos: {transform.position}, aim: {target - transform.position}");
                dir.y = 0;
                dir = dir.normalized;

                world.AddComponent<RotateCommand>(index)
                    .Initialize(x => { x.RotateDirection = dir; return x; });

                Vector3 distToPlayer = player.position - humanoid.transform.position;
                distToPlayer.y = 0;
                if (distToPlayer.sqrMagnitude <= 3f)
                {
                    world.AddComponent<AttackCommand>(index);
                    dir = Vector3.zero;
                }

                world.AddComponent<MoveCommand>(index)
                    .Initialize(x => { x.MoveDirection = dir; return x; });
            }
        }
    }
}