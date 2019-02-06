using SurvivalShooterECS.Components.Hybrid;
using Unity.Entities;
using UnityEngine;

namespace SurvivalShooterECS.Systems.Hybrid.Input
{
    public class PlayerRotationSystem : ComponentSystem
    {
        private struct Filter
        {
            public Transform transform;
            public RotationComponent rotationComponent;
        }
        
        protected override void OnUpdate()
        {
            ComponentGroupArray<Filter> filteredEntities = GetEntities<Filter>();

            Vector3 mousePosition = UnityEngine.Input.mousePosition;
            Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);
            int layerMask = LayerMask.GetMask("Floor");
            float rayMaxDistance = 1000.0f;

            RaycastHit hit;
            
            if (Physics.Raycast(cameraRay, out hit, rayMaxDistance, layerMask))
            {
                for (int i = 0; i < filteredEntities.Length ; i++)
                {
                    var entity = filteredEntities[i];
                    Vector3 forward = hit.point - entity.transform.position;

                    Quaternion rotation = Quaternion.LookRotation(forward);

                    entity.rotationComponent.Value = new Quaternion(0, rotation.y, 0, rotation.w).normalized;
                }
            }
        }
    }
}