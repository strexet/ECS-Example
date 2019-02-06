using SurvivalShooterECS.Components.Hybrid;
using Unity.Entities;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Hybrid.PlayerMovement
{
    public class PlayerMovementSystem : ComponentSystem
    {
        private struct Filter
        {
            public Rigidbody rigidbody;
            public InputComponent inputComponent;
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.deltaTime;
            
            float moveSpeed = 3.0f;
            float moveAmount = moveSpeed * deltaTime;
            
            var filteredEntities = GetEntities<Filter>();
            for (int i = 0; i < filteredEntities.Length; i++)
            {
                var entity = filteredEntities[i];

                var moveVector = new Vector3(entity.inputComponent.Horizontal, 0, entity.inputComponent.Vertical)
                    .normalized;
                var newPosition = moveAmount * moveVector + entity.rigidbody.position;
                
                entity.rigidbody.MovePosition(newPosition);
            }
        }
    }
}