using UnityEngine;

namespace Controllers
{
    public class TowerSpawner : MonoBehaviour
    {
        public GameObject[] towerPrefabs; // Array of tower prefabs for each type

        private void Start()
        {
            // Retrieve the selected tower type from PlayerPrefs
            var selectedTowerType = PlayerPrefs.GetInt("SelectedTowerType", 0);

            // Instantiate the selected tower prefab at the desired position
            Instantiate(towerPrefabs[selectedTowerType], transform.position, Quaternion.identity);
        }
    }
}