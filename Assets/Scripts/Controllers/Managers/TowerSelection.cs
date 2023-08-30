using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Managers
{
    public class TowerSelection : MonoBehaviour
    {
        public int towerTypeIndex;
        public Button button;

        private void Start()
        {
            button.onClick.AddListener(SelectTower);
        }

        private void SelectTower()
        {
            // Store the selected tower type index in PlayerPrefs
            PlayerPrefs.SetInt("SelectedTowerType", towerTypeIndex);
        }
    }
}