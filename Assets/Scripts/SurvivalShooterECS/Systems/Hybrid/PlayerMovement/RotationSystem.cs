using SurvivalShooterECS.Components.Hybrid;
using Unity.Entities;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Hybrid.PlayerMovement
{
    public class RotationSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentArray<RotationComponent> RotationComponents;
            public ComponentArray<Rigidbody> Rigidbodies;
        }

        [Inject] private Data data;
        
        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                Quaternion rotation = data.RotationComponents[i].Value;
                data.Rigidbodies[i].MoveRotation(rotation.normalized);
            }
        }
    }
}