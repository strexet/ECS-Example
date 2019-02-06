using SurvivalShooterECS.Components.Hybrid;
using Unity.Entities;

namespace SurvivalShooterECS.Systems.Hybrid.Input
{
    public class InputSystem : ComponentSystem 
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentArray<InputComponent> inputComponents;
        }

        [Inject]
        private Data data;
        
        protected override void OnUpdate()
        {
            var horizontalInput = UnityEngine.Input.GetAxis("Horizontal");
            var verticalInput = UnityEngine.Input.GetAxis("Vertical");

            for (int i = 0; i < data.Length; i++)
            {
                var inputComponent = data.inputComponents[i];

                inputComponent.Horizontal = horizontalInput;
                inputComponent.Vertical = verticalInput;
            }
        }
    }
}