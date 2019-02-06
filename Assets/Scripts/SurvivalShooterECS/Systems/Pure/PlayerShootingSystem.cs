using SurvivalShooterECS.Components.Pure;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Pure
{
    public class PlayerShootingSystem : JobComponentSystem
    {
        // IJobParallelFor is basically a for-loop that runs in parallel.
        private struct PlayerShootingJob : IJobParallelFor
        {
            [ReadOnly] public EntityArray EntityArray;
            public float CurrentTime;
            
            // 1) It is unsafe to add, delete or modify entities when a job is still running.
            //    Command buffers allow you to queue up this type of actions.
            //    Unity will perform this actions at appropriate time.
            // 2) Concurrent buffers are specifically used for jobs that run in parallel or concurrently.
            public EntityCommandBuffer.Concurrent EntityCommandBuffer; 
            
            public void Execute(int index)
            {
                EntityCommandBuffer.AddComponent(index, EntityArray[index], new Firing{FiredAt = CurrentTime});
            }
        }

        private struct Data
        {
            // 1) Length field is populated with the value that reflects the size of the EntityArray.
            // 2) It is readonly field, because if we were to modify that value by accident,
            //    the compiler would through an error.
            public readonly int Length;
            
            // EntityArray is populated with all of the entities that meet the component specification. 
            public EntityArray Entities;
            
            // ComponentDataArray tells Unity to the struct with entities that have the Weapon component.
            public ComponentDataArray<Weapon> Weapons;
            
            // SubtractiveComponent tells Unity to ignore any entities that have the Firing component.
            public SubtractiveComponent<Firing> Firings;
        }

        // 1) BarrierSystem is a special class that we can use to perform actions on our entities.
        // 2) Barriers live on the main thread,
        //    so we can use them to create a CommandBuffer that queues up and executes actions when it is safe.
        private class PlayerShootingBarrier : BarrierSystem
        {
        }
        

        [Inject] private Data data;
        [Inject] private PlayerShootingBarrier barrier;
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // TODO: Make the alternative work.
            // ALTERNATIVE:
//            EntityArchetypeQuery query = new EntityArchetypeQuery
//            {
//                All = new ComponentType[] {typeof(Weapon)},
//                None = new ComponentType[] {typeof(Firing)}
//            };
//
//            NativeArray<ArchetypeChunk> chunks = EntityManager.CreateArchetypeChunkArray(query, Allocator.Persistent);
//            
//            // do some stuff with chunks here...
//            JobHandle handle = (...) .Schedule(...);
//            handle.Complete();
//            
//            chunks.Dispose();
            
            
            if (UnityEngine.Input.GetButton("Fire1"))
            {
                int batchSize = 64;
                return new PlayerShootingJob
                {
                    EntityArray = data.Entities,
                    EntityCommandBuffer = barrier.CreateCommandBuffer().ToConcurrent(),
                    CurrentTime = Time.time
                }.Schedule(data.Length, batchSize, inputDeps);
            }

            return inputDeps;
        }
    }
    
    
}