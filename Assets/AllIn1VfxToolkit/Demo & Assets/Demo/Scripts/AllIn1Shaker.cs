using UnityEngine;
using UnityEngine.Serialization;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1Shaker : MonoBehaviour
    {
        [SerializeField] Vector3 maximumTranslationShake = Vector3.one;
        [SerializeField] Vector3 maximumAngularShake = Vector3.one * 15;
        [SerializeField] float shakeFrequency = 25;
        [SerializeField] float shakeSmoothingExponent = 1;
        [SerializeField] float shakeRecoverPerSecond = 1;

        public static AllIn1Shaker i;
        private float currentShakeAmount;
        private float seed;

        private void Awake()
        {
            if (i != null && i != this) Destroy(gameObject);
            else  i = this;
            
            seed = Random.value;
        }

        private void Update()
        {
            float shake = SmoothShakeToApply();
            ShakePosition(shake);
            ShakeRotation(shake);
            currentShakeAmount = Mathf.Clamp01(currentShakeAmount - shakeRecoverPerSecond * Time.deltaTime);
        }

        private float SmoothShakeToApply()
        {
            float shake = Mathf.Pow(currentShakeAmount, shakeSmoothingExponent);
            return shake;
        }

        private void ShakeRotation(float shake)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(
                maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * shakeFrequency) * 2 - 1),
                maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * shakeFrequency) * 2 - 1),
                maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * shakeFrequency) * 2 - 1)
            ) * shake);
        }

        private void ShakePosition(float shake)
        {
            transform.localPosition = new Vector3(
                maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * shakeFrequency) * 2 - 1),
                maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * shakeFrequency) * 2 - 1),
                maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * shakeFrequency) * 2 - 1)
            ) * shake;
        }

        public void DoCameraShake(float shakeAmount)
        {
            currentShakeAmount = shakeAmount;
        }
    }
}