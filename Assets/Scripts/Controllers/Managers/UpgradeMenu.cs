/*
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Managers
{
    public class UpgradeMenu : MonoBehaviour
    {
        [SerializeField] private Text goldText; // UI text displaying current Gold balance
        [SerializeField] private Button weaponDamageButton;
        [SerializeField] private Button projectileSpeedButton;
        // Add buttons for other upgrade types...

        // Data structure to track attribute levels (similar to UpgradeOption)
        private UpgradeProgression weaponDamageProgression;
        private UpgradeProgression projectileSpeedProgression;
        // Add progressions for other upgrade types...

        // Gold cost for each upgrade
        private int weaponDamageUpgradeCost = 100;
        private int projectileSpeedUpgradeCost = 150;
        // Add costs for other upgrade types...

        // Initialize attribute levels and UI
        private void Start()
        {
            // Load attribute levels and Gold balance from storage (e.g., PlayerPrefs)

            // Initialize UI text
            UpdateGoldText();

            // Add click event handlers for each upgrade button
            weaponDamageButton.onClick.AddListener(UpgradeWeaponDamage);
            projectileSpeedButton.onClick.AddListener(UpgradeProjectileSpeed);
            // Add event handlers for other upgrade types...
        }

        // Update the displayed Gold balance
        private void UpdateGoldText()
        {
            goldText.text = "Gold: " + PlayerData.CurrentGold; // Replace with your method for getting the current Gold balance
        }

        // Upgrade Weapon Damage
        private void UpgradeWeaponDamage()
        {
            if (PlayerData.CurrentGold >= weaponDamageUpgradeCost)
            {
                // Deduct Gold cost
                PlayerData.CurrentGold -= weaponDamageUpgradeCost;

                // Increase attribute level
                weaponDamageProgression.currentLevel++;

                // Update UI and save data
                UpdateGoldText();
                SavePlayerData();
            }
            else
            {
                // Handle insufficient Gold
                Debug.Log("Not enough Gold to upgrade Weapon Damage.");
            }
        }

        // Upgrade Projectile Speed (similar functions for other upgrade types)
        private void UpgradeProjectileSpeed()
        {
            if (PlayerData.CurrentGold >= projectileSpeedUpgradeCost)
            {
                PlayerData.CurrentGold -= projectileSpeedUpgradeCost;
                projectileSpeedProgression.currentLevel++;
                UpdateGoldText();
                SavePlayerData();
            }
            else
            {
                Debug.Log("Not enough Gold to upgrade Projectile Speed.");
            }
        }

        // Save player data (Gold balance and attribute levels)
        private void SavePlayerData()
        {
            // Save the updated Gold balance and attribute levels to storage (e.g., PlayerPrefs)
            // Make sure to handle saving and loading data appropriately
        }
    }
}
*/
