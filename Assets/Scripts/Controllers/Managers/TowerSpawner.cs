using Containers;
using Controllers.Player_Controllers;
using UnityEngine;

namespace Controllers.Managers
{
    public class TowerSpawner : MonoBehaviour
    {
        public GameObject[] towerPrefabs;

        private void Awake()
        {
            /*// Read player attribute values from PlayerPrefs
            var selectedTowerType = PlayerPrefs.GetInt("SelectedTowerType", 0);
            var weaponDamageLevel = PlayerPrefs.GetInt(UpgradeType.WeaponDamage.ToString(), 0);
            var projectileSpeedLevel = PlayerPrefs.GetInt(UpgradeType.ProjectileSpeed.ToString(), 0);
            var weaponCooldownLevel = PlayerPrefs.GetInt(UpgradeType.WeaponCooldown.ToString(), 0);
            var aoeEffectLevel = PlayerPrefs.GetInt(UpgradeType.AoeEffect.ToString(), 0);

            // Instantiate the player with the correct attribute values
            GameObject player = Instantiate(towerPrefabs[selectedTowerType], transform.position, Quaternion.identity);
            
            // Access the PlayerController script on the instantiated player
            var playerController = player.GetComponent<PlayerController>();
            
            // Initialize the player
            playerController.playerData.Initialize();

            // Set the player's attribute levels
            playerController.playerData.attributeLevels[UpgradeType.WeaponDamage] = weaponDamageLevel;
            playerController.playerData.attributeLevels[UpgradeType.ProjectileSpeed] = projectileSpeedLevel;
            playerController.playerData.attributeLevels[UpgradeType.WeaponCooldown] = weaponCooldownLevel;
            playerController.playerData.attributeLevels[UpgradeType.AoeEffect] = aoeEffectLevel;*/
            
            var selectedTowerType = PlayerPrefs.GetInt("SelectedTowerType", 0);
            GameObject player = Instantiate(towerPrefabs[selectedTowerType], transform.position, Quaternion.identity);

            // Access the PlayerController script on the instantiated player
            var playerController = player.GetComponent<PlayerController>();
            
            // Initialize the player
            playerController.playerData.Initialize();

            // Set the player's attribute levels
            playerController.playerData.attributeLevels[UpgradeType.WeaponDamage] = PlayerPrefs.GetInt(UpgradeType.WeaponDamage.ToString(), 0);
            playerController.playerData.attributeLevels[UpgradeType.ProjectileSpeed] = PlayerPrefs.GetInt(UpgradeType.ProjectileSpeed.ToString(), 0);
            playerController.playerData.attributeLevels[UpgradeType.WeaponCooldown] = PlayerPrefs.GetInt(UpgradeType.WeaponCooldown.ToString(), 0);
            playerController.playerData.attributeLevels[UpgradeType.AoeEffect] = PlayerPrefs.GetInt(UpgradeType.AoeEffect.ToString(), 0);
        }
    }
}