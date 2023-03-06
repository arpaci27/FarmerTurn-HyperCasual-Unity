using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class All1DemoProjectileObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject[] projectileObstacles;
        private Dropdown projectileObstacleDropdown;
        
        private void Start()
        {
            projectileObstacleDropdown = GetComponent<Dropdown>();
            DropdownValueChanged();
        }

        public void DropdownValueChanged()
        {
            SetProjectileObstacleN(projectileObstacleDropdown.value);
        }

        private void SetProjectileObstacleN(int nIndex)
        {
            for(int i = 0; i < projectileObstacles.Length; i++) projectileObstacles[i].SetActive(i == nIndex);
        }
    }
}