using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class TowerSelection : MonoBehaviour
    {
        public int towerTypeIndex; // Assign a unique index to each tower type button
        public Button button; // Reference to the button component

        private void Start()
        {
            // Add a listener to the button's onClick event
            button.onClick.AddListener(SelectTower);
        }

        private void SelectTower()
        {
            // Store the selected tower type index in PlayerPrefs
            PlayerPrefs.SetInt("SelectedTowerType", towerTypeIndex);
        }
    }
}