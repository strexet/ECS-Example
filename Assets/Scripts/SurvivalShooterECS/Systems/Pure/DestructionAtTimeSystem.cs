using SurvivalShooterECS.Components.Pure;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Pure
{
    public class DestructionAtTimeSystem : JobComponentSystem
    {
        private struct DestructionAtTimeJob : IJobParallelFor
        {
            [ReadOnly] public EntityArray Entities;
            public EntityCommandBuffer.Concurrent EntityCommandBuffer;
            public ComponentDataArray<DestroyAtTime> DestroyAtTimeComponents;
            public float CurrentTime;
            
            public void Execute(int index)
            {
                if (CurrentTime > DestroyAtTimeComponents[index].DestructionTime)
                {
                    EntityCommandBuffer.DestroyEntity(index, Entities[index]);
                }
            }            
        }
        
        private struct Data
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<DestroyAtTime> DestroyAtTimeComponents;
        }

        private class DestructionAtTimeBarrier : BarrierSystem
        {
        }

        [Inject] private Data data;
        [Inject] private DestructionAtTimeBarrier barrier;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            int innerLoopBatchCount = 64;
            
            return new DestructionAtTimeJob
            {
                Entities = data.Entities,
                DestroyAtTimeComponents = data.DestroyAtTimeComponents,
                EntityCommandBuffer = barrier.CreateCommandBuffer().ToConcurrent(),
                CurrentTime = Time.time
            }.Schedule(data.Length, innerLoopBatchCount, inputDeps);
        }
    }
}