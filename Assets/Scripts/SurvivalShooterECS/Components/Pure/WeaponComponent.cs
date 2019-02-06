using Unity.Entities;

namespace SurvivalShooterECS.Components.Pure
{
    public struct Weapon : IComponentData
    {
    }
    
    public class WeaponComponent : ComponentDataWrapper<Weapon> // this wrapper derives from MonoBehaviour
    // ComponentDataWrapper uses its generic type parameter to add pure ECS components to GameObject's based entities.
    {
        
    }
}