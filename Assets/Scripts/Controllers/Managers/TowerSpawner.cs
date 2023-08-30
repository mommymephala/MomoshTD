using UnityEngine;

namespace Controllers.Managers
{
    public class TowerSpawner : MonoBehaviour
    {
        public GameObject[] towerPrefabs;

        private void Start()
        {
            var selectedTowerType = PlayerPrefs.GetInt("SelectedTowerType", 0);

            Instantiate(towerPrefabs[selectedTowerType], transform.position, Quaternion.identity);
        }
    }
}