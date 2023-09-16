using System;
using UnityEngine;
using TMPro;
using Containers;

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
        public TextMeshProUGUI towerMaxHpGoldCostText;
        public TextMeshProUGUI towerMaxHpCurrentValueText;
        public TextMeshProUGUI healthRegenAmountGoldCostText;
        public TextMeshProUGUI healthRegenAmountCurrentValueText;

        private void Start()
        {
            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                if (PlayerPrefs.HasKey(upgradeType.ToString()))
                {
                    var savedLevel = PlayerPrefs.GetInt(upgradeType.ToString());
                    playerData.attributeLevels[upgradeType] = savedLevel;
                    UpdateUIElements(upgradeType);
                }
            }
        }

        private void UpdateUIElements(UpgradeType upgradeType)
        {
            var currentLevel = playerData.attributeLevels[upgradeType];
            var totalGoldCost = goldCostPerLevel * (currentLevel + 1);

            switch (upgradeType)
            {
                case UpgradeType.WeaponDamage:
                    weaponDamageGoldCostText.text = totalGoldCost.ToString();
                    weaponDamageCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.ProjectileSpeed:
                    projectileSpeedGoldCostText.text = totalGoldCost.ToString();
                    projectileSpeedCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.WeaponCooldown:
                    weaponCooldownGoldCostText.text = totalGoldCost.ToString();
                    weaponCooldownCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.AoeEffect:
                    aoeEffectGoldCostText.text = totalGoldCost.ToString();
                    aoeEffectCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.TowerMaxHp:
                    towerMaxHpGoldCostText.text = totalGoldCost.ToString();
                    towerMaxHpCurrentValueText.text = currentLevel.ToString();
                    break;
                case UpgradeType.HealthRegenAmount:
                    healthRegenAmountGoldCostText.text = totalGoldCost.ToString();
                    healthRegenAmountCurrentValueText.text = currentLevel.ToString();
                    break;
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

        public void UpgradeTowerMaxHp()
        {
            UpgradeAttribute(UpgradeType.TowerMaxHp);
        }

        public void UpgradeHealthRegenAmount()
        {
            UpgradeAttribute(UpgradeType.HealthRegenAmount);
        }

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
            //TODO: Fix Upgrades
            var currentLevel = playerData.attributeLevels[upgradeType];
            var totalGoldCost = goldCostPerLevel * (currentLevel + 1);

            if (gold.totalGold >= totalGoldCost && currentLevel < playerData.MaxLevelForAttribute(upgradeType) && currentLevel < playerData.MaxPermanentLevel)
            {
                gold.totalGold -= totalGoldCost;

                playerData.attributeLevels[upgradeType]++;

                PlayerPrefs.SetInt("TotalGold", gold.totalGold);
                PlayerPrefs.SetInt(upgradeType.ToString(), playerData.attributeLevels[upgradeType]);
                UpdateUIElements(upgradeType);

                PlayerPrefs.Save();
            }
        }
    }
}
