using UnityEngine;

namespace AllIn1VfxToolkit
{
    [CreateAssetMenu(fileName = "All1VfxParticleHelperTemplate", menuName = "AllIn1Vfx/ParticleHelperTemplate")]
    public class AllIn1ParticleHelperSO : ScriptableObject
    {
        //General Options
        public bool matchDurationToLifetime = false;
        public bool randomRotation = false;
        public float minLifetime = 5f, maxLifetime = 5f;
        public float minSpeed = 5f, maxSpeed = 5f;
        public float minSize = 1f, maxSize = 1f;
        public ParticleSystem.MinMaxGradient startColor;
        
        //Emission Options
        public bool isBurst = false;
        public int minNumberOfParticles = 10, maxNumberOfParticles = 10;
        
        //Shape Options
        public AllIn1ParticleHelperComponent.EmissionShapes currEmissionShape = AllIn1ParticleHelperComponent.EmissionShapes.Cone;
        
        //Lifetime Options
        public AllIn1ParticleHelperComponent.LifetimeSettings colorLifetime = AllIn1ParticleHelperComponent.LifetimeSettings.Descendent;
        public AllIn1ParticleHelperComponent.LifetimeSettings sizeLifetime = AllIn1ParticleHelperComponent.LifetimeSettings.Ascendant;
    }
}
