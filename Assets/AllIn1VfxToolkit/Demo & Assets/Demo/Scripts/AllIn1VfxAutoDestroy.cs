using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1VfxAutoDestroy : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 1f;

        private void Start()
        {
            Destroy(gameObject, destroyTime);
        }
    }
}