using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class All1VfxRandomTimeSeed : MonoBehaviour
    {
        [SerializeField] private float minSeedValue = 0;
        [SerializeField] private float maxSeedValue = 100f;

        private void Start()
        {
            MaterialPropertyBlock properties = new MaterialPropertyBlock();
            properties.SetFloat("_TimingSeed", Random.Range(minSeedValue, maxSeedValue));
            GetComponent<Renderer>().SetPropertyBlock(properties);
        }
    }
}