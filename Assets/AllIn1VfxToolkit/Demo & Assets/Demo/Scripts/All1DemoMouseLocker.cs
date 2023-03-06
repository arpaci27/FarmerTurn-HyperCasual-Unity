using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [RequireComponent(typeof(AllIn1MouseRotate))]
    public class All1DemoMouseLocker : MonoBehaviour
    {
        [SerializeField] private KeyCode mouseLockerKey = KeyCode.L;
        [SerializeField] private Image lockButtonImage;
        [SerializeField] private Color lockButtonColor;

        private AllIn1MouseRotate allIn1MouseRotate;
        private AllIn1DemoScaleTween lockedTween;
        private Text pausedButtonText;
        private bool currentlyLocked = false;

        private void Start()
        {
            allIn1MouseRotate = GetComponent<AllIn1MouseRotate>();
            lockedTween = lockButtonImage.GetComponent<AllIn1DemoScaleTween>();
            pausedButtonText = lockButtonImage.GetComponentInChildren<Text>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(mouseLockerKey)) DoMouseLockToggle();
        }

        public void DoMouseLockToggle()
        {
            currentlyLocked = !currentlyLocked;
            allIn1MouseRotate.enabled = !currentlyLocked;
            lockedTween.ScaleUpTween();
            if(currentlyLocked)
            {
                pausedButtonText.text = "Unlock Camera";
                lockButtonImage.color = lockButtonColor;
            }
            else
            {
                pausedButtonText.text = "Lock Camera";
                lockButtonImage.color = Color.white;
            }
        }
    }
}