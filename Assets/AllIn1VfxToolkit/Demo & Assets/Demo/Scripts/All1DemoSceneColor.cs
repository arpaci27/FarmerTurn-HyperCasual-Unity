using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [RequireComponent(typeof(Dropdown))]
    public class All1DemoSceneColor : MonoBehaviour
    {
        [SerializeField] private Color[] sceneColors;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float cameraColorMult = 1f;
        [SerializeField] private MeshRenderer floorMeshRenderer;
        [SerializeField] private float floorColorMult = 1f;
        [SerializeField] private float fogColorMult = 1f;
        
        private Dropdown sceneColorDropdown;
        private Material floorMaterial;
        
        private void Start()
        {
            sceneColorDropdown = GetComponent<Dropdown>();
            floorMaterial = floorMeshRenderer.material;
            DropdownValueChanged();
        }

        public void DropdownValueChanged()
        {
            SetSceneColor(sceneColorDropdown.value);
        }

        private void SetSceneColor(int nIndex)
        {
            targetCamera.backgroundColor = sceneColors[nIndex] * cameraColorMult;
            floorMaterial.color = sceneColors[nIndex] * floorColorMult;
            RenderSettings.fogColor = sceneColors[nIndex] * fogColorMult;
        }
    }
}
