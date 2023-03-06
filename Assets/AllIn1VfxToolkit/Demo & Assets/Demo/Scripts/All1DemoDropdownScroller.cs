using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [RequireComponent(typeof(Dropdown))]
    public class All1DemoDropdownScroller : MonoBehaviour
    {
        [SerializeField] private int dropdownElementCount;
        [SerializeField] private KeyCode nextDropdownElementKey = KeyCode.M;
        
        private Dropdown dropdown;

        private void Start()
        {
            dropdown = GetComponent<Dropdown>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(nextDropdownElementKey)) NextDropdownElements();
        }

        private void NextDropdownElements()
        {
            int nextValue = dropdown.value + 1;
            if(nextValue < 0) nextValue = dropdownElementCount - 1;
            if(nextValue >= dropdownElementCount) nextValue = 0;
            dropdown.value = nextValue;
        }
    }
}