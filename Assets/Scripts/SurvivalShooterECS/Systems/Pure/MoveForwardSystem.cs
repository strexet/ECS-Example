using SurvivalShooterECS.Components.Pure;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Pure
{
    public class MoveForwardSystem : JobComponentSystem
    {
        private struct MoveForwardJob : IJobProcessComponentData<Position, Rotation, MoveSpeed>
        {
            public void Execute(ref Position position, ref Rotation rotation, ref MoveSpeed moveSpeed)
            {
                Quaternion rot = rotation.Value;
                Vector3 pos = position.Value;
                Vector3 forward = rot * Vector3.forward;

                position.Value = pos + forward * moveSpeed.Value;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new MoveForwardJob().Schedule(this, inputDeps);
        }
    }
}