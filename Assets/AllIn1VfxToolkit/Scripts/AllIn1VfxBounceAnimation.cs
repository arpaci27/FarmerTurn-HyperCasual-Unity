using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class AllIn1VfxBounceAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 targetOffset = Vector3.up;
        [SerializeField] private float speed = 1f;
        
        private Vector3 startPosition, animationMovementVector;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            animationMovementVector = targetOffset * ((Mathf.Sin(Time.time * speed) + 1f) / 2f);
            transform.position = startPosition + animationMovementVector;
        }
    }
}