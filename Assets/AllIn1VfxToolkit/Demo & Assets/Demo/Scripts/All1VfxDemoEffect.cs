using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [CreateAssetMenu(fileName = "All1VfxDemoEffect", menuName = "AllIn1Vfx/DemoEffect")]
    public class All1VfxDemoEffect : ScriptableObject
    {
        [Header("Can be replayed?")]
        public bool onlyOneAtATime;
        public bool canBePlayedAgain = true;
        public float randomSpreadRadius;
        public float cooldown = 0.25f;
        
        [Space, Header("Chooses between Static Effect and Shoot Projectile")]
        public bool isShootProjectile;
        
        [Space, Header("Static Effect Property")]
        public GameObject effectPrefab;
        public bool spawnTouchingFloor;
        
        [Space, Header("Shoot Projectile Properties")]
        public float projectileSpeed = 15f;
        public GameObject projectilePrefab;
        public GameObject muzzleFlashPrefab;
        public GameObject impactPrefab;
        
        [Space, Header("Camera Shake")]
        public bool doCameraShake;
        public float mainEffectShakeAmount = 1f;
        [Header("Only if Shoot Projectile")] public float projectileImpactShakeAmount = 1f;
        
        [Space, Header("Other Properties")]
        public Vector3 positionOffset;
        public float scaleMultiplier = 1f;
    }
}