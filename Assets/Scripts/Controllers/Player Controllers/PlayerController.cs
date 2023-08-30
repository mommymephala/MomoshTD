using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Containers;
using Controllers.Enemy_Controllers;
using Controllers.Managers;
using Controllers.Weapon_Controllers;

namespace Controllers.Player_Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask coinLayer;
        [SerializeField] public List<BaseWeaponController> weaponControllers;
        [SerializeField] private List<BaseWeaponController> weaponPrefabsList;
        [SerializeField] private Transform weaponHolder;
        private int _howManyLevelsUp;
        
        //Time
        private bool _isGamePaused;
        
        [Header("XP Calculation")]
        [SerializeField] private float baseXpRequirement;
        [SerializeField] private float xpMultiplierForNextLv;
        [SerializeField] private float collectionRadius;
        
        //xp/level containers
        private readonly List<int> _xpLevelThresholds = new List<int>();
        private int _currentLevel = 1;
        private const int MaxLevel = 100;

        private float _currentHealth;
        
        private int _playerXp;
        private int _playerGold;
        //public int totalGold;
        
        private Camera _camera;

        // HP regeneration
        private const float HpRegenInterval = 1.0f;
        private float _nextHpRegenTime;
        
        //UPGRADABLE
        private float _maxCurrentHealth;
        private float _bonusHpRegen;

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
            //LoadPlayerProgress();
        }
        
        // private void LoadPlayerProgress()
        // {
        //     totalGold = PlayerPrefs.GetInt("TotalGold", 0);
        // }

        private void Update()
        {
            if (_isGamePaused)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                CollectCoinsAtClick();
            }
            
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
            if (_currentLevel >= _xpLevelThresholds.Count || !(_playerXp >= _xpLevelThresholds[_currentLevel - 1]))
                return false;
            
            _currentLevel++;
            return true;
        }

        public void OnLevelUp()
        {
            if (_howManyLevelsUp <= 0) return;
            _howManyLevelsUp--;
            PauseGame();
            _isGamePaused = true;
            Debug.Log("Level Up! Current Level: " + (_currentLevel - _howManyLevelsUp));

            var upgradeOptions = GenerateUpgradeOptions();

            // Randomly select two upgrade options
            var optionIndex1 = Random.Range(0, upgradeOptions.Count);
            var optionIndex2 = Random.Range(0, upgradeOptions.Count - 1);
            if (optionIndex2 >= optionIndex1)
                optionIndex2++;

            UpgradeOption option1 = upgradeOptions[optionIndex1];
            UpgradeOption option2 = upgradeOptions[optionIndex2];

            PresentUpgradeChoices(option1, option2);
        }
        
        private List<UpgradeOption> GenerateUpgradeOptions()
        {
            var options = new List<UpgradeOption>
            {
                new UpgradeOption
                {
                    type = UpgradeType.WeaponDamage,
                    description = "Increase Damage"
                },
                new UpgradeOption
                {
                    type = UpgradeType.ProjectileSpeed,
                    description = "Increase Projectile Speed"
                },
                new UpgradeOption
                {
                    type = UpgradeType.TowerMaxHp,
                    description = "Increase Tower Max HP"
                },
                new UpgradeOption
                {
                    type = UpgradeType.HealthRegenAmount,
                    description = "Increase Health Regeneration"
                },
                new UpgradeOption
                {
                    type = UpgradeType.AoeEffect,
                    description = "Improve AOE Effect"
                },
                new UpgradeOption
                {
                    type = UpgradeType.WeaponCooldown,
                    description = "Reduce Cooldown"
                }
            };
            if (CanAttachNewWeapon())
            {
                options.Add(new UpgradeOption 
                { 
                    type = UpgradeType.AddNewWeapon,
                    description = "Add New Weapon" 
                });
            }

            return options;
        }
        
        private void PresentUpgradeChoices(UpgradeOption option1, UpgradeOption option2)
        {
            var upgradeOptions = new List<UpgradeOption> { option1, option2 };
            UIManager.Instance.ShowUpgradePanel(upgradeOptions, OnUpgradeChoiceSelected);
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
                    _maxCurrentHealth += 10; // Increase max health by 10
                    _currentHealth = Mathf.Min(_currentHealth + 10, _maxCurrentHealth); // Also heal the tower
                    Debug.Log("Increased Tower Max HP! New Tower Max Health: " + _maxCurrentHealth);
                    break;

                case UpgradeType.HealthRegenAmount:
                    _bonusHpRegen += 0.1f; // Increase health regeneration amount by 0.1
                    Debug.Log("Increased Health Regeneration! New Bonus HP Regen: " + _bonusHpRegen);
                    break;
                
                case UpgradeType.AddNewWeapon:
                    AttachNewWeapon();
                    break;
                        
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private bool CanAttachNewWeapon()
        {
            return weaponHolder != null && weaponPrefabsList.Any(weaponPrefab => !IsWeaponAlreadyAttached(weaponHolder, weaponPrefab));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void AttachNewWeapon()
        {
            if (weaponHolder == null)
            {
                Debug.LogError("WeaponHolder not found.");
                return;
            }

            foreach (BaseWeaponController weaponPrefab in weaponPrefabsList)
            {
                if (IsWeaponAlreadyAttached(weaponHolder, weaponPrefab)) continue;
    
                Vector3 localPosition = weaponPrefab.transform.localPosition;
                Quaternion localRotation = weaponPrefab.transform.localRotation;
    
                GameObject newWeapon = Instantiate(weaponPrefab.gameObject, weaponHolder);
    
                newWeapon.transform.localPosition = localPosition;
                newWeapon.transform.localRotation = localRotation;
    
                weaponControllers.Add(newWeapon.GetComponent<BaseWeaponController>());
                break;
            }
        }

        private static bool IsWeaponAlreadyAttached(Component weaponHolder, Component weaponPrefab)
        {
            return weaponHolder.GetComponentsInChildren<BaseWeaponController>().Any(child => child.gameObject.CompareTag(weaponPrefab.tag));
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void OnUpgradeChoiceSelected(UpgradeOption chosenUpgrade)
        {
            ApplyUpgrade(chosenUpgrade);
            ResumeGame();
            _isGamePaused = false;
            
            UIManager.Instance.HideUpgradePanel();
        }
        
        private static void PauseGame()
        {
            TimeControl.Instance.previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private static void ResumeGame()
        {
            Time.timeScale = TimeControl.Instance.previousTimeScale;
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
        
        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        private void CollectCoinsAtClick()
        {
            Vector3 clickPosition = GetMouseClickPosition();

            var colliders = Physics.OverlapSphere(clickPosition, collectionRadius, coinLayer);
            foreach (Collider coinCollider in colliders)
            {
                CollectCoin(coinCollider.gameObject);
                if (CheckForLevelUp())
                {
                    _howManyLevelsUp++;
                }
            }
            OnLevelUp();
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
                }
            }

            if (coin.CompareTag("Gold"))
            {
                var goldCoin = coin.GetComponent<GoldCoinController>();
                if (goldCoin != null)
                {
                    var goldAmount = GoldCoinController.GetGoldAmount();
                    AddGold(goldAmount);
                }
            }
            
            Destroy(coin);
        }

        private void AddXp(int xpAmount)
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
            EnemySpawnManager.Instance.gameObject.SetActive(false);
        }
    }
}