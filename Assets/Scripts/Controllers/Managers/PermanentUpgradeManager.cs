/*using System;
using System.Collections.Generic;
using Containers;
using Controllers.Weapon_Controllers;
using UnityEngine;

namespace Controllers.Managers
{
    public class PermanentUpgradeManager : MonoBehaviour
    {
        [SerializeField] public List<BaseWeaponController> weaponControllers;

        private void Awake()
        {
            PlayerPrefs.GetInt("TotalGold");
        }
        
        private void ApplyUpgrade(UpgradeOption upgrade)
        {
            switch (upgrade.type)
            {
                case UpgradeType.WeaponDamage:
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.damageModifier += 0.1f;
                    }
                    Debug.Log("Upgraded Weapon Damage!");
                    break;

                case UpgradeType.ProjectileSpeed:
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.currentProjectileSpeedModifier += 0.1f;
                    }
                    Debug.Log("Increased Projectile Speed!");
                    break;

                case UpgradeType.WeaponCooldown:
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.currentCooldownModifier -= 0.1f;
                    }
                    Debug.Log("Reduced Weapon Cooldown!");
                    break;

                case UpgradeType.AoeEffect:
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.areaModifier += 0.1f;
                    }
                    Debug.Log("Improved AOE Effect!");
                    break;

                case UpgradeType.TowerMaxHp:
                    maxCurrentHealth += 10;
                    currentHealth = Mathf.Min(currentHealth + 10, maxCurrentHealth); // Also heal the tower
                    Debug.Log("Increased Tower Max HP! New Tower Max Health: " + maxCurrentHealth);
                    break;

                case UpgradeType.HealthRegenAmount:
                    _bonusHpRegen += 0.1f;
                    Debug.Log("Increased Health Regeneration! New Bonus HP Regen: " + _bonusHpRegen);
                    break;
                
                case UpgradeType.AddNewWeapon:
                    AttachNewWeapon();
                    break;
                        
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}*/