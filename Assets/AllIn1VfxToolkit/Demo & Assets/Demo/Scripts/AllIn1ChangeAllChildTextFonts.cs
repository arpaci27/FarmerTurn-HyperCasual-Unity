using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1ChangeAllChildTextFonts : MonoBehaviour
    {
        [SerializeField] private Font newFont;
        [SerializeField] private bool changeFontOnStart;

        private void Start()
        {
            if(changeFontOnStart) ChangeFonts();
        }

        [ContextMenu("ChangeFonts")]
        private void ChangeFonts()
        {
            Text[] canvasTexts = GetComponentsInChildren<Text>();
            for(int i = 0; i < canvasTexts.Length; i++) canvasTexts[i].font = newFont;
        }
    }
}
