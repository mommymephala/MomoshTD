using System.Collections.Generic;
using Containers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public GameObject upgradePanel;
        public Button upgradeButtonPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        //TODO: Implement this method
        public void ShowUpgradePanel(List<UpgradeOption> upgradeOptions, System.Action<UpgradeOption> onUpgradeChosen)
        {
            upgradePanel.SetActive(true);

            // Create buttons for each upgrade option
            foreach (UpgradeOption option in upgradeOptions)
            {
                Button button = Instantiate(upgradeButtonPrefab, upgradePanel.transform);
                button.GetComponentInChildren<Text>().text = option.description;

                // Capture the option as a parameter for the callback
                button.onClick.AddListener(() => onUpgradeChosen(option));
            }
        }
    }
}