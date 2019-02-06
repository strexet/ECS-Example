using SurvivalShooterECS.Components.Pure;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Pure
{
    public class CleanupFiringSystem : JobComponentSystem
    {
        private struct CleanupFiringJob : IJobParallelFor
        {
            [ReadOnly] public EntityArray Entities;
            public EntityCommandBuffer.Concurrent EntityCommandBuffer;
            public ComponentDataArray<Firing> Firings;
            public float CurrentTime;
            public float FiringDelay;
            
            public void Execute(int index)
            {
                if (CurrentTime - Firings[index].FiredAt > FiringDelay)
                {
                    EntityCommandBuffer.RemoveComponent<Firing>(index, Entities[index]);
                }
            }            
        }
        
        private struct Data
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<Firing> Firings;
        }

        private class CleanupFiringBarrier : BarrierSystem
        {
        }

        [Inject] private Data data;
        [Inject] private CleanupFiringBarrier barrier;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            int innerLoopBatchCount = 64;
            return new CleanupFiringJob
            {
                Entities = data.Entities,
                Firings = data.Firings,
                EntityCommandBuffer = barrier.CreateCommandBuffer().ToConcurrent(),
                CurrentTime = Time.time,
                FiringDelay = Bootstrap.FiringDelay
            }.Schedule(data.Length, innerLoopBatchCount, inputDeps);
        }
    }
}