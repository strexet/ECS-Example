using SurvivalShooterECS.Components.Pure;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Pure
{
    public class FiringSystem : JobComponentSystem
    {
        // IJobProcessComponentData process entities based on their components.
        // We can tell it which components to look for through its generic properties.
        private struct FiringJob : IJobParallelFor
        {
            [ReadOnly] public EntityArray Entities;
            public EntityCommandBuffer.Concurrent EntityCommandBuffer;
            public ComponentDataArray<Position> Positions;
            public ComponentDataArray<Rotation> Rotations;
            public float BulletSpeed;
            public float DestructionTime;

            public void Execute(int index)
            {
                var entity = EntityCommandBuffer.CreateEntity(index);

                // MeshInstanceRendererSystem only operates on entities tha have both a MeshInstanceRenderer component
                // and a TransformMatrix component.
                EntityCommandBuffer.AddSharedComponent(index, entity, Bootstrap.BulletRenderer);

                EntityCommandBuffer.AddComponent(index, entity, new MoveSpeed {Value = BulletSpeed});

                EntityCommandBuffer.AddComponent(index, entity, Positions[index]);
                EntityCommandBuffer.AddComponent(index, entity, Rotations[index]);
                EntityCommandBuffer.AddComponent(
                    index, entity, new Scale {Value = new float3(0.45f, 0.45f, 0.45f)});
                EntityCommandBuffer.AddComponent(
                    index, entity, new DestroyAtTime {DestructionTime = DestructionTime});
            }
        }

        private class FiringBarrier : BarrierSystem { }

        [Inject] private FiringBarrier barrier;
        private ComponentGroup componentGroup;

        protected override void OnCreateManager()
        {
            // Filters entities by components: 
            componentGroup = GetComponentGroup(
                ComponentType.Create<Firing>(),
                ComponentType.Create<Position>(),
                ComponentType.Create<Rotation>());

            // Says to react only to changed/added components (removed is not supported yet).
            componentGroup.SetFilterChanged(ComponentType.Create<Firing>());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            int innerLoopBatchCount = 64;

            FiringJob firingJob = new FiringJob
            {
                Entities = componentGroup.GetEntityArray(),
                EntityCommandBuffer = barrier.CreateCommandBuffer().ToConcurrent(),
                Positions = componentGroup.GetComponentDataArray<Position>(),
                Rotations = componentGroup.GetComponentDataArray<Rotation>(),
                BulletSpeed = Bootstrap.BulletSpeed,
                DestructionTime = Time.time + Bootstrap.BulletLifetime
            };

            return firingJob.Schedule(componentGroup.CalculateLength(), innerLoopBatchCount, inputDeps);
        }
    }
}