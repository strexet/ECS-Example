using Unity.Entities;

namespace SurvivalShooterECS.Components.Pure
{
    public struct DestroyAtTime : IComponentData
    {
        public float DestructionTime;
    }
}