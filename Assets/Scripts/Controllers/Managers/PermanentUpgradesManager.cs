/*using System;
using System.Collections.Generic;
using Containers;
using Controllers.Player_Controllers;
using UnityEngine;

namespace Controllers.Managers
{
    public class PermanentUpgradesManager : MonoBehaviour
    {
        public static PermanentUpgradesManager Instance;
        public PlayerController playerController;
        public List<PermanentUpgradeOption> permanentUpgradeOptions = new List<PermanentUpgradeOption>();

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

        public bool CanAffordUpgrade(PermanentUpgradeOption upgradeOption)
        {
            return playerController.totalGold >= upgradeOption.cost && !upgradeOption.isUnlocked;
        }

        public void PurchaseUpgrade(PermanentUpgradeOption upgradeOption)
        {
            if (!CanAffordUpgrade(upgradeOption)) return;
            playerController.totalGold -= upgradeOption.cost;
            upgradeOption.isUnlocked = true;
            ApplyUpgrade(upgradeOption);
        }

        private void ApplyUpgrade(PermanentUpgradeOption upgradeOption)
        {
            switch (upgradeOption.type)
            {
                case UpgradeType.WeaponDamage:
                    // Apply weapon damage upgrade logic
                    break;
                case UpgradeType.ProjectileSpeed:
                    // Apply projectile speed upgrade logic
                    break;
                // Add cases for other upgrade types
                case UpgradeType.WeaponCooldown:
                    break;
                case UpgradeType.AoeEffect:
                    break;
                case UpgradeType.TowerMaxHp:
                    break;
                case UpgradeType.HealthRegenAmount:
                    break;
                case UpgradeType.AddNewWeapon:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}*/