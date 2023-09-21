using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Containers;
using Controllers.Weapon_Controllers;

namespace Controllers.Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        [HideInInspector] public PlayerData playerData;
        public GetGold gold;
        public int goldCostPerLevel = 10;

        public TextMeshProUGUI weaponDamageGoldCostText;
        public TextMeshProUGUI weaponDamageCurrentValueText;
        public TextMeshProUGUI projectileSpeedGoldCostText;
        public TextMeshProUGUI projectileSpeedCurrentValueText;
        public TextMeshProUGUI weaponCooldownGoldCostText;
        public TextMeshProUGUI weaponCooldownCurrentValueText;
        public TextMeshProUGUI aoeEffectGoldCostText;
        public TextMeshProUGUI aoeEffectCurrentValueText;
        // public TextMeshProUGUI towerMaxHpGoldCostText;
        // public TextMeshProUGUI towerMaxHpCurrentValueText;
        // public TextMeshProUGUI healthRegenAmountGoldCostText;
        // public TextMeshProUGUI healthRegenAmountCurrentValueText;
        
        public List<BaseWeaponController> weaponControllers;

        private void Awake()
        {
            playerData = new PlayerData();
            playerData.Initialize();
        }

        private void Start()
        {
            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                var savedLevel = PlayerPrefs.GetInt(upgradeType.ToString());
                playerData.attributeLevels[upgradeType] = savedLevel;

                UpdateUIElements(upgradeType);
            }
        }

        private void UpdateUIElements(UpgradeType upgradeType)
        {
            var currentLevel = playerData.attributeLevels[upgradeType];
            var totalGoldCost = Mathf.RoundToInt(goldCostPerLevel * Mathf.Pow(2f, currentLevel));

            switch (upgradeType)
            {
                case UpgradeType.WeaponDamage:
                    weaponDamageGoldCostText.text = (currentLevel < 5) ? totalGoldCost.ToString() : "MAX";
                    weaponDamageCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.ProjectileSpeed:
                    projectileSpeedGoldCostText.text = (currentLevel < 5) ? totalGoldCost.ToString() : "MAX";
                    projectileSpeedCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.WeaponCooldown:
                    weaponCooldownGoldCostText.text = (currentLevel < 5) ? totalGoldCost.ToString() : "MAX";
                    weaponCooldownCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.AoeEffect:
                    aoeEffectGoldCostText.text = (currentLevel < 5) ? totalGoldCost.ToString() : "MAX";
                    aoeEffectCurrentValueText.text = currentLevel.ToString();
                    break;
                // case UpgradeType.TowerMaxHp:
                //     towerMaxHpGoldCostText.text = (currentLevel < 5) ? totalGoldCost.ToString() : "MAX";
                //     towerMaxHpCurrentValueText.text = currentLevel.ToString();
                //     break;
                // case UpgradeType.HealthRegenAmount:
                //     healthRegenAmountGoldCostText.text = (currentLevel < 5) ? totalGoldCost.ToString() : "MAX";
                //     healthRegenAmountCurrentValueText.text = currentLevel.ToString();
                //     break;
            }
        }

        public void UpgradeWeaponDamage()
        {
            UpgradeAttribute(UpgradeType.WeaponDamage);
        }

        public void UpgradeProjectileSpeed()
        {
            UpgradeAttribute(UpgradeType.ProjectileSpeed);
        }

        public void UpgradeWeaponCooldown()
        {
            UpgradeAttribute(UpgradeType.WeaponCooldown);
        }

        public void UpgradeAoeEffect()
        {
            UpgradeAttribute(UpgradeType.AoeEffect);
        }

        // public void UpgradeTowerMaxHp()
        // {
        //     UpgradeAttribute(UpgradeType.TowerMaxHp);
        // }
        //
        // public void UpgradeHealthRegenAmount()
        // {
        //     UpgradeAttribute(UpgradeType.HealthRegenAmount);
        // }

        public void ResetAttributes()
        {
            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                playerData.attributeLevels[upgradeType] = 0;
                PlayerPrefs.SetInt(upgradeType.ToString(), 0);
                UpdateUIElements(upgradeType);
            }

            PlayerPrefs.Save();
        }

        public void AddGold(int amount)
        {
            gold.totalGold += amount;

            PlayerPrefs.SetInt("TotalGold", gold.totalGold);
            PlayerPrefs.Save();
        }

        private void UpgradeAttribute(UpgradeType upgradeType)
        {
            var currentLevel = playerData.attributeLevels[upgradeType];
            var totalGoldCost = Mathf.RoundToInt(goldCostPerLevel * Mathf.Pow(2f, currentLevel));

            if (gold.totalGold < totalGoldCost || currentLevel >= playerData.MaxPermanentLevel) return;
            
            gold.totalGold -= totalGoldCost;
            playerData.attributeLevels[upgradeType]++;

            PlayerPrefs.SetInt("TotalGold", gold.totalGold);
            PlayerPrefs.SetInt(upgradeType.ToString(), playerData.attributeLevels[upgradeType]);

            switch (upgradeType)
            {
                case UpgradeType.WeaponDamage:
                    var damageModifier = 0.2f;
                    
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.damageModifier += damageModifier;
                    }

                    Debug.Log("Weapon Damage Modifier Increased by: " + damageModifier);
                    break;

                case UpgradeType.ProjectileSpeed:
                    var projectileSpeedModifier = 0.1f;

                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.currentProjectileSpeedModifier += projectileSpeedModifier;
                    }

                    Debug.Log("Projectile Speed Modifier Increased by: " + projectileSpeedModifier);
                    break;

                case UpgradeType.WeaponCooldown:
                    var cooldownModifier = 0.1f;

                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.currentCooldownModifier -= cooldownModifier;
                    }

                    Debug.Log("Weapon Cooldown Modifier Decreased by: " + cooldownModifier);
                    break;

                case UpgradeType.AoeEffect:
                    var aoeModifier = 0.2f;

                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.areaModifier += aoeModifier;
                    }

                    Debug.Log("AOE Effect Modifier Increased by: " + aoeModifier);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
                    
            UpdateUIElements(upgradeType);

            PlayerPrefs.Save();
        }
    }
}