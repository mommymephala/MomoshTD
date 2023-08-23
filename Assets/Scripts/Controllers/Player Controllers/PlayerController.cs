using System;
using System.Collections.Generic;
using UnityEngine;
using Containers;
using Controllers.Managers;
using Controllers.Weapon_Controllers;
using Random = UnityEngine.Random;

namespace Controllers.Player_Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask coinLayer;
        [SerializeField] private List<BaseWeaponController> weaponControllers;
        
        [Header("XP Calculation")]
        [SerializeField] private float baseXpRequirement;
        [SerializeField] private float xpMultiplierForNextLv;
        [SerializeField] private float collectionRadius;

        //UPGRADABLE
        private float _maxCurrentHealth;
        
        //xp/level containers
        private readonly List<int> _xpLevelThresholds = new List<int>();
        private int _currentLevel = 1;
        
        private float _currentHealth;
        
        private float _playerXp;
        private int _playerGold;
        
        private float _nextHpRegenTime;
        private float _bonusHpRegen;
        
        private Camera _camera;

        // Constants for HP regeneration
        private const float HpRegenInterval = 1.0f;
        private const int MaxLevel = 100;

        private void Awake()
        {
            _camera = Camera.main;
            _currentHealth = _maxCurrentHealth = towerData.maxHp;
            _nextHpRegenTime = Time.time + HpRegenInterval;
            _bonusHpRegen = towerData.baseHpRegen;
        }
        
        private void Start()
        {
            GenerateXpLevelThresholds();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CollectCoinsAtClick();
            }
            
            CheckForLevelUp();
            
            // Check for HP regeneration
            if (!(_currentHealth < _maxCurrentHealth) || !(Time.time >= _nextHpRegenTime)) return;
            RegenerateHp();
            _nextHpRegenTime = Time.time + HpRegenInterval;
        }
        
        private void GenerateXpLevelThresholds()
        {
            _xpLevelThresholds.Clear();
            var xpThreshold = baseXpRequirement;

            for (var level = 1; level <= MaxLevel; level++)
            {
                _xpLevelThresholds.Add(Mathf.FloorToInt(xpThreshold));
                xpThreshold *= xpMultiplierForNextLv;
            }
        }
        
        private bool CheckForLevelUp()
        {
            // Check if the player has enough XP to level up
            if (_currentLevel >= _xpLevelThresholds.Count || !(_playerXp >= _xpLevelThresholds[_currentLevel - 1]))
                return false; // Return the flag indicating whether a level-up occurred
            _currentLevel++;
            Debug.Log("Level Up! Current Level: " + _currentLevel);

            return true; // Return the flag indicating whether a level-up occurred
        }

        private void OnLevelUp()
        {
            if (!CheckForLevelUp()) return;
            PauseGame();
            
            var upgradeOptions = GenerateUpgradeOptions(towerData, weaponControllers);

            // Randomly select two upgrade options
            var optionIndex1 = Random.Range(0, upgradeOptions.Count);
            var optionIndex2 = Random.Range(0, upgradeOptions.Count - 1);
            if (optionIndex2 >= optionIndex1)
                optionIndex2++;

            UpgradeOption option1 = upgradeOptions[optionIndex1];
            UpgradeOption option2 = upgradeOptions[optionIndex2];
            
            // TODO: Present the upgrade options to the player and let them choose
            // TODO: Handle selection UI here

            ResumeGame();
        }

        private static void PauseGame()
        {
            Time.timeScale = 0;
        }
        
        private static void ResumeGame()
        {
            Time.timeScale = 1;
        }

        private void PresentUpgradeChoices()
        {
            var upgradeOptions = GenerateUpgradeOptions(towerData, weaponControllers);
            UIManager.Instance.ShowUpgradePanel(upgradeOptions, OnUpgradeChoiceSelected);
            // TODO: Actually do the UI
        }
        
        private static List<UpgradeOption> GenerateUpgradeOptions(TowerData towerData, List<BaseWeaponController> weaponControllers)
        {
            var options = new List<UpgradeOption>();

            foreach (BaseWeaponController weaponController in weaponControllers)
            {
                // Check if the weapon can increase damage
                if (weaponController.weaponData.baseDamage * towerData.baseDmgModifier < weaponController.weaponData.baseDamage)
                {
                    options.Add(new UpgradeOption
                    {
                        type = UpgradeType.WeaponDamage,
                        description = "Increase " + weaponController.weaponData.name + " Damage"
                    });
                }

                // Check if the weapon can increase projectile speed
                if (weaponController.weaponData.baseProjectileSpeed * towerData.baseProjectileSpeedModifier < weaponController.weaponData.baseProjectileSpeed)
                {
                    options.Add(new UpgradeOption
                    {
                        type = UpgradeType.ProjectileSpeed,
                        description = "Increase " + weaponController.weaponData.name + " Projectile Speed"
                    });
                }
        
                // TODO: Add other upgrade options

            }

            return options;
        }

        private static void OnUpgradeChoiceSelected(UpgradeOption chosenUpgrade)
        {
            ApplyUpgrade(chosenUpgrade);
            ResumeGame();
        }

        private static void ApplyUpgrade(UpgradeOption upgrade)
        {
            switch (upgrade.type)
            {
                case UpgradeType.WeaponDamage:
                    break;
                case UpgradeType.ProjectileSpeed:
                    break;
                case UpgradeType.AoeEffect:
                    break;
                case UpgradeType.TowerMaxHp:
                    break;
                case UpgradeType.HealthRegenSpeed:
                    break;
                case UpgradeType.AddNewWeapon:
                    break;
                // TODO: Handle other upgrade types as needed
                // TODO: Think about a default statement
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RegenerateHp()
        {
            var hpToRegen = Mathf.RoundToInt(towerData.baseHpRegen + _bonusHpRegen);
            _currentHealth = Mathf.Min(_currentHealth + hpToRegen, _maxCurrentHealth);
        }
        
        private Vector3 GetMouseClickPosition()
        {
            Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out RaycastHit hit) ? hit.point : Vector3.zero;
        }
        
        private void CollectCoinsAtClick()
        {
            Vector3 clickPosition = GetMouseClickPosition();

            var colliders = Physics.OverlapSphere(clickPosition, collectionRadius, coinLayer);
            foreach (Collider coinCollider in colliders)
            {
                CollectCoin(coinCollider.gameObject);
            }
        }

        private void CollectCoin(GameObject coin)
        {
            if (coin.CompareTag("XpGem"))
            {
                var xpGem = coin.GetComponent<XpGemController>();
                if (xpGem != null)
                {
                    var xpAmount = xpGem.GetXpAmount();
                    AddXp(xpAmount);
                    Debug.Log("XP: " + _playerXp);
                }
            }

            if (coin.CompareTag("Gold"))
            {
                var goldCoin = coin.GetComponent<GoldCoinController>();
                if (goldCoin != null)
                {
                    var goldAmount = goldCoin.GetGoldAmount();
                    AddGold(goldAmount);
                    Debug.Log("GOLD: " + _playerGold);
                }
            }
            
            Destroy(coin);
        }

        private void AddXp(float xpAmount)
        {
            _playerXp += xpAmount;
        }

        private void AddGold(int goldAmount)
        {
            _playerGold += goldAmount;
        }

        public void TowerTakeDamage(int damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
