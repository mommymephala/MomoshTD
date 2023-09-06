using UnityEngine;

namespace Controllers.Managers
{
    public class TabManager : MonoBehaviour
    {
        public GameObject mainMenuPanel;
        public GameObject upgradesPanel;
        public GameObject towerSelectionPanel;

        private void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
            upgradesPanel.SetActive(false);
            towerSelectionPanel.SetActive(false);
        }

        public void ShowUpgrades()
        {
            mainMenuPanel.SetActive(false);
            upgradesPanel.SetActive(true);
            towerSelectionPanel.SetActive(false);
        }

        public void ShowTowerSelection()
        {
            mainMenuPanel.SetActive(false);
            upgradesPanel.SetActive(false);
            towerSelectionPanel.SetActive(true);
        }
    }
}
