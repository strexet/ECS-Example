using Unity.Rendering;
using UnityEngine;

namespace SurvivalShooterECS
{
        public class Bootstrap : MonoBehaviour
        {
                // MeshInstanceRenderer-s are components that are used by the MeshInstanceRendererSystem.
                // The MeshInstanceRendererSystem is responsible for rendering 3D models in the scene.
                // (It is one of the handful of systems that comes bundled in with the Entities package)  
                public static RenderMesh BulletRenderer;
                public static float BulletSpeed;
                public static float FiringDelay;
                public static float BulletLifetime;

                [SerializeField] private RenderMesh bulletRenderer;
                [SerializeField] private float bulletSpeed = 0.5f;
                [SerializeField] private float firingDelay = 0.5f;
                [SerializeField] private float bulletLifetime = 5f;
                

                private void Awake()
                {
                        BulletRenderer = bulletRenderer;
                        BulletSpeed = bulletSpeed;
                        FiringDelay = firingDelay;
                        BulletLifetime = bulletLifetime;
                }
        }
}