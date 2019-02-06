using Unity.Entities;

namespace SurvivalShooterECS.Components.Pure
{
    public struct Firing : IComponentData
    {
        public float FiredAt;
    }
}