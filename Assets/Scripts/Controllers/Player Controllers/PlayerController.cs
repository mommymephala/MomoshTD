using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using Containers;
using Controllers.Enemy_Controllers;
using Controllers.Managers;
using Controllers.Weapon_Controllers;
using Pickups;

namespace Controllers.Player_Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask pickupLayer;
        [SerializeField] private GameObject xpPickupFX;
        [SerializeField] private GameObject goldPickupFX;
        [SerializeField] private GameObject healthPickupFX;
        [SerializeField] public List<BaseWeaponController> weaponControllers;
        [SerializeField] private List<BaseWeaponController> weaponPrefabsList;
        [SerializeField] private Transform weaponHolder;
        [HideInInspector] public PlayerData playerData;
        private int _howManyLevelsUp;
        
        //Time
        private bool _isGamePaused;
        private bool _hasEndedTheRun;
        
        [Header("XP Calculation")]
        [SerializeField] private float baseXpRequirement;
        [SerializeField] private float xpMultiplierForNextLv;
        [SerializeField] private float collectionRadius;

        [Header("Events")] 
        public float gameTime;
        private EnemySpawnManager _spawnManager;
        [HideInInspector] public float gameEndTime = 300f;
        private float _bigEnemySpawnTime;
        private float _bigEnemySpawnChance;
        private const float InitialSpawnChance = 0.2f;
        private const float MaxSpawnChance = 1.0f;
        private const float ChanceIncreaseInterval = 15f;

        [Header("UI")] 
        private HealthBar _healthBar;
        private GameObject _youDiedPanel;
        private GameObject _youWonPanel;
        private TextMeshProUGUI  _gameTimeText;
        private TextMeshProUGUI _currentGoldText;
        
        //xp/level containers
        private readonly List<int> _xpLevelThresholds = new List<int>();
        private int _currentLevel = 1;
        private const int MaxLevel = 20;

        [HideInInspector] public float currentHealth;
        
        //UPGRADABLE
        public float maxCurrentHealth;
        private float _bonusHpRegen;
        
        private int _playerXp;
        private int _playerGold;

        // HP regeneration
        private const float HpRegenInterval = 1.0f;
        private float _nextHpRegenTime;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _spawnManager = FindObjectOfType<EnemySpawnManager>();

            GameObject gameTimeObject = GameObject.Find("Time Display");
            _gameTimeText = gameTimeObject.GetComponent<TextMeshProUGUI>();

            _youDiedPanel = GameObject.Find("YOU DIED PANEL");
            _youWonPanel = GameObject.Find("YOU WON PANEL");

            _healthBar = FindObjectOfType<HealthBar>();

            GameObject currentGoldObject = GameObject.Find("CurrentGold");
            _currentGoldText = currentGoldObject.GetComponent<TextMeshProUGUI>();

            playerData = new PlayerData();
            playerData.Initialize();
        }

        private void Start()
        {
            _youDiedPanel.SetActive(false);
            _youWonPanel.SetActive(false);
            currentHealth = maxCurrentHealth = towerData.maxHp;
            _healthBar.SetMaxHealth(maxCurrentHealth);
            _nextHpRegenTime = Time.time + HpRegenInterval;
            _bonusHpRegen = towerData.baseHpRegen;

            gameTime = 0f;

            _bigEnemySpawnTime = 0f;
            _bigEnemySpawnChance = InitialSpawnChance;

            _playerGold = 0;
            _currentGoldText.text = _playerGold.ToString();

            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                if (upgradeType != UpgradeType.AddNewWeapon && PlayerPrefs.HasKey(upgradeType.ToString()))
                {
                    var savedLevel = PlayerPrefs.GetInt(upgradeType.ToString());

                    // Check if the saved level is greater than 0, then subtract 1 level
                    if (savedLevel > 0)
                    {
                        savedLevel--;
                        playerData.attributeLevels[upgradeType] = savedLevel;
                        ApplyUpgradeAtStart(upgradeType, weaponControllers[0]); // Apply upgrades based on saved levels
                    }
                }
            }
            
            GenerateXpLevelThresholds();
        }

        private void Update()
        {
            if (_isGamePaused)
            {
                return;
            }

            gameTime += Time.deltaTime;
            EndTheRun();
            
            UpdateGameTimeText();
            
            // Check for big enemy spawn
            _bigEnemySpawnTime += Time.deltaTime;
            UpdateBigEnemySpawnChance();

            if (_bigEnemySpawnTime >= ChanceIncreaseInterval)
            {
                var calculatedSpawnChance = CalculateBigEnemySpawnChance(InitialSpawnChance, MaxSpawnChance, gameTime);

                if (Random.value <= calculatedSpawnChance)
                {
                    _spawnManager.SpawnBigEnemy(_bigEnemySpawnChance);

                    _bigEnemySpawnTime = 0f;
                    _bigEnemySpawnChance = 0f;

                    StartCoroutine(StartBigEnemySpawnChanceIncrease());
                }
            }
    
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)))
            {
                CollectPickupsAtClick();
            }
    
            // Check for HP regeneration
            if (!(currentHealth < maxCurrentHealth) || !(Time.time >= _nextHpRegenTime)) return;
            RegenerateHp();
            _nextHpRegenTime = Time.time + HpRegenInterval;
        }
        
        [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
        private void UpdateGameTimeText()
        {
            var totalSeconds = Mathf.FloorToInt(gameTime);
            var minutes = Mathf.FloorToInt(totalSeconds / 60);
            var seconds = totalSeconds % 60;
            var gameTimeFormatted = $"{minutes:D2}:{seconds:D2}";
            _gameTimeText.text = gameTimeFormatted;
        }
        
        private float CalculateBigEnemySpawnChance(float initialChance, float maxChance, float elapsedTime)
        {
            var normalizedTime = Mathf.Clamp01(elapsedTime / ChanceIncreaseInterval);

            _bigEnemySpawnChance = Mathf.Lerp(initialChance, maxChance, normalizedTime);

            return _bigEnemySpawnChance;
        }

        private IEnumerator StartBigEnemySpawnChanceIncrease()
        {
            yield return new WaitForSeconds(ChanceIncreaseInterval);
            _bigEnemySpawnChance = InitialSpawnChance;
        }

        private void UpdateBigEnemySpawnChance()
        {
            if (_bigEnemySpawnChance < MaxSpawnChance)
            {
                _bigEnemySpawnChance += Time.deltaTime / ChanceIncreaseInterval;
            }
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

            var upgradeOptions = GenerateUpgradeOptions();

            var optionIndex1 = Random.Range(0, upgradeOptions.Count);
            var optionIndex2 = Random.Range(0, upgradeOptions.Count - 1);
            if (optionIndex2 >= optionIndex1)
                optionIndex2++;

            UpgradeOption option1 = upgradeOptions[optionIndex1];
            UpgradeOption option2 = upgradeOptions[optionIndex2];

            PresentUpgradeChoices(option1, option2);
        }
        
        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        private List<UpgradeOption> GenerateUpgradeOptions()
        {
            var options = new List<UpgradeOption>();

            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                // Skip generating the "Add New Weapon" option here
                if (upgradeType == UpgradeType.AddNewWeapon)
                {
                    continue;
                }

                var currentLevel = playerData.attributeLevels[upgradeType];
        
                // Check if the current level is less than 10 before adding the upgrade option
                if (currentLevel < 10)
                {
                    options.Add(new UpgradeOption
                    {
                        type = upgradeType,
                        description = GetUpgradeDescription(upgradeType),
                    });
                }
            }
    
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
        
        private static string GetUpgradeDescription(UpgradeType upgradeType)
        {
            return upgradeType switch
            {
                UpgradeType.WeaponDamage => "Increase Damage",
                UpgradeType.ProjectileSpeed => "Increase Projectile Speed",
                // UpgradeType.TowerMaxHp => "Increase Tower Max HP",
                // UpgradeType.HealthRegenAmount => "Increase Health Regeneration",
                UpgradeType.AoeEffect => "Improve AOE Effect",
                UpgradeType.WeaponCooldown => "Reduce Cooldown",
                UpgradeType.AddNewWeapon => "Add New Weapon",
                _ => ""
            };
        }
        
        private void PresentUpgradeChoices(UpgradeOption option1, UpgradeOption option2)
        {
            var upgradeOptions = new List<UpgradeOption> { option1, option2 };
            InGameUpgradeUI.Instance.ShowUpgradePanel(upgradeOptions, OnUpgradeChoiceSelected);
        }

        // Apply attribute upgrades
        private void ApplyUpgrade(UpgradeType upgradeType)
        {
            // Retrieve the current level from player data
            var currentLevel = playerData.attributeLevels[upgradeType];
            
            // Determine the maximum level allowed for this upgrade
            var maxLevel = playerData.MaxLevelForAttribute(upgradeType);

            // Check if the current level is already at the maximum
            if (currentLevel >= maxLevel)
            {
                Debug.Log(upgradeType + " is already at max level (" + currentLevel + ")");
                return;
            }

            // Increment the upgrade level
            playerData.attributeLevels[upgradeType]++;

            // Log the upgrade
            Debug.Log("Upgraded " + upgradeType + " to Level " + playerData.attributeLevels[upgradeType]);

            // Now apply the upgrade based on the new level
            switch (upgradeType)
            {
                case UpgradeType.WeaponDamage:
                    // Example: Increase weapon damage by 20% per level
                    var damageModifier = 0.2f;
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.damageModifier += damageModifier;
                    }
                    Debug.Log("Weapon Damage Modifier Increased by: " + damageModifier);
                    break;

                case UpgradeType.ProjectileSpeed:
                    // Example: Increase projectile speed by 10% per level
                    var speedModifier = 0.1f;
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.currentProjectileSpeedModifier += speedModifier;
                    }
                    Debug.Log("Projectile Speed Modifier Increased by: " + speedModifier);
                    break;

                case UpgradeType.WeaponCooldown:
                    // Example: Decrease weapon cooldown by 10% per level
                    var cooldownModifier = 0.1f;
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.currentCooldownModifier -= cooldownModifier;
                    }
                    Debug.Log("Weapon Cooldown Modifier Decreased by: " + cooldownModifier);
                    break;

                case UpgradeType.AoeEffect:
                    // Example: Increase AOE effect by 20% per level
                    var aoeModifier = 0.2f;
                    foreach (BaseWeaponController weaponController in weaponControllers)
                    {
                        weaponController.areaModifier += aoeModifier;
                    }
                    Debug.Log("AOE Effect Modifier Increased by: " + aoeModifier);
                    break;

                case UpgradeType.AddNewWeapon:
                    AttachNewWeapon();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ApplyUpgradeAtStart(UpgradeType upgradeType, BaseWeaponController weaponController)
        {
            // Retrieve the current level from player data
            var currentLevel = playerData.attributeLevels[upgradeType] + 1;

            // Determine the maximum level allowed for this upgrade
            var maxLevel = playerData.MaxLevelForAttribute(upgradeType);

            // Check if the current level is already at the maximum
            if (currentLevel >= maxLevel)
            {
                Debug.Log(upgradeType + " is already at max level (" + currentLevel + ")");
                return;
            }

            // Increment the upgrade level
            playerData.attributeLevels[upgradeType]++;

            // Log the upgrade
            Debug.Log("Upgraded " + upgradeType + " to Level " + playerData.attributeLevels[upgradeType]);

            // Now apply the upgrade based on the new level
            switch (upgradeType)
            {
                case UpgradeType.WeaponDamage:
                    // Example: Increase weapon damage by 20% per level
                    var damageModifier = currentLevel * 0.2f;
                    weaponController.damageModifier += damageModifier;
                    Debug.Log("Weapon Damage Modifier Increased by: " + damageModifier);
                    break;

                case UpgradeType.ProjectileSpeed:
                    // Example: Increase projectile speed by 10% per level
                    var speedModifier = currentLevel * 0.1f;
                    weaponController.currentProjectileSpeedModifier += speedModifier;
                    Debug.Log("Projectile Speed Modifier Increased by: " + speedModifier);
                    break;

                case UpgradeType.WeaponCooldown:
                    // Example: Decrease weapon cooldown by 10% per level
                    var cooldownModifier = currentLevel * 0.1f;
                    weaponController.currentCooldownModifier -= cooldownModifier;
                    Debug.Log("Weapon Cooldown Modifier Decreased by: " + cooldownModifier);
                    break;

                case UpgradeType.AoeEffect:
                    // Example: Increase AOE effect by 20% per level
                    var aoeModifier = currentLevel * 0.2f;
                    weaponController.areaModifier += aoeModifier;
                    Debug.Log("AOE Effect Modifier Increased by: " + aoeModifier);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ApplyUpgradeForNewWeapon(UpgradeType upgradeType, BaseWeaponController weaponController)
        {
            // Retrieve the current level from player data
            var currentLevel = playerData.attributeLevels[upgradeType];

            // Determine the maximum level allowed for this upgrade
            var maxLevel = playerData.MaxLevelForAttribute(upgradeType);

            // Check if the current level is already at the maximum
            if (currentLevel >= maxLevel)
            {
                Debug.Log(upgradeType + " is already at max level (" + currentLevel + ")");
                return;
            }

            // Increment the upgrade level
            //playerData.attributeLevels[upgradeType]++;

            // Log the upgrade
            Debug.Log("Upgraded " + upgradeType + " to Level " + playerData.attributeLevels[upgradeType]);

            // Now apply the upgrade based on the new level
            switch (upgradeType)
            {
                case UpgradeType.WeaponDamage:
                    // Example: Increase weapon damage by 20% per level
                    var damageModifier = currentLevel * 0.2f;
                    weaponController.damageModifier += damageModifier;
                    Debug.Log("Weapon Damage Modifier Increased by: " + damageModifier);
                    break;

                case UpgradeType.ProjectileSpeed:
                    // Example: Increase projectile speed by 10% per level
                    var speedModifier = currentLevel * 0.1f;
                    weaponController.currentProjectileSpeedModifier += speedModifier;
                    Debug.Log("Projectile Speed Modifier Increased by: " + speedModifier);
                    break;

                case UpgradeType.WeaponCooldown:
                    // Example: Decrease weapon cooldown by 10% per level
                    var cooldownModifier = currentLevel * 0.1f;
                    weaponController.currentCooldownModifier -= cooldownModifier;
                    Debug.Log("Weapon Cooldown Modifier Decreased by: " + cooldownModifier);
                    break;

                case UpgradeType.AoeEffect:
                    // Example: Increase AOE effect by 20% per level
                    var aoeModifier = currentLevel * 0.2f;
                    weaponController.areaModifier += aoeModifier;
                    Debug.Log("AOE Effect Modifier Increased by: " + aoeModifier);
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

                var newWeaponController = newWeapon.GetComponent<BaseWeaponController>();

                // Apply upgrades based on saved levels from PlayerPrefs
                foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
                {
                    if (PlayerPrefs.HasKey(upgradeType.ToString()))
                    {
                        var savedLevel = PlayerPrefs.GetInt(upgradeType.ToString());

                        // Check if the saved level is greater than 0
                        if (savedLevel > 0)
                        {
                            ApplyUpgradeForNewWeapon(upgradeType, newWeaponController);
                        }
                    }
                }

                weaponControllers.Add(newWeaponController);
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
            // Assuming chosenUpgrade contains the UpgradeType and level, update accordingly
            ApplyUpgrade(chosenUpgrade.type);
            ResumeGame();
            _isGamePaused = false;
    
            InGameUpgradeUI.Instance.HideUpgradePanel();
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
            currentHealth = Mathf.Min(currentHealth + hpToRegen, maxCurrentHealth);
        }
        
        private Vector3 GetMouseClickPosition()
        {
            Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out RaycastHit hit) ? hit.point : Vector3.zero;
        }
        
        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        private void CollectPickupsAtClick()
        {
            Vector3 clickPosition = GetMouseClickPosition();

            var colliders = Physics.OverlapSphere(clickPosition, collectionRadius, pickupLayer);
            foreach (Collider coinCollider in colliders)
            {
                CollectPickup(coinCollider.gameObject);
                if (CheckForLevelUp())
                {
                    _howManyLevelsUp++;
                }
            }
            OnLevelUp();
        }

        private void CollectPickup(GameObject pickup)
        {
            Vector3 pickupPosition = pickup.transform.position;

            if (pickup.CompareTag("XpGem"))
            {
                var xpGem = pickup.GetComponent<XpGemController>();
                if (xpGem != null)
                {
                    var xpAmount = xpGem.GetXpAmount();
                    GameObject vfx = Instantiate(xpPickupFX, pickupPosition, Quaternion.identity);
                    AddXp(xpAmount);
                    Destroy(vfx,1.5f);
                }
            }

            if (pickup.CompareTag("Gold"))
            {
                var goldCoin = pickup.GetComponent<GoldCoinController>();
                if (goldCoin != null)
                {
                    var goldAmount = GoldCoinController.GetGoldAmount();
                    _currentGoldText.text = _playerGold.ToString();
                    GameObject vfx = Instantiate(goldPickupFX, pickupPosition, Quaternion.identity);
                    AddGold(goldAmount);
                    Destroy(vfx,1.5f);
                }
            }

            if (pickup.CompareTag("HealthGem"))
            {
                var healthGem = pickup.GetComponent<HealthPickup>();
                if (healthGem != null)
                {
                    var healAmount = healthGem.GetHealAmount(maxCurrentHealth);
                    GameObject vfx = Instantiate(healthPickupFX, pickupPosition, Quaternion.identity);
                    Heal(healAmount);
                    Destroy(vfx,1.5f);
                }
            }

            Destroy(pickup);
        }

        private void Heal(int healAmount)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxCurrentHealth);
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
            currentHealth -= damageAmount;
            _healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                PlayerDie();
            }
        }

        private void EndTheRun()
        {
            if (!(gameTime >= gameEndTime) || _hasEndedTheRun) return;

            var currentTotalGold = PlayerPrefs.GetInt("TotalGold", 0);
            if (_playerGold != 0)
            {
                currentTotalGold += _playerGold - 1;
                PlayerPrefs.SetInt("TotalGold", currentTotalGold);
            }
            else
            {
                currentTotalGold += _playerGold;
                PlayerPrefs.SetInt("TotalGold", currentTotalGold);
            }
            
            EnemySpawnManager.Instance.gameObject.SetActive(false);
            var enemyControllers = FindObjectsOfType<EnemyController>();
            foreach (EnemyController enemyController in enemyControllers)
            {
                enemyController.Die();
            }

            if (_youWonPanel != null)
            {
                _gameTimeText.gameObject.SetActive(false);
                _healthBar.gameObject.SetActive(false);
                _youWonPanel.SetActive(true);
            }

            _hasEndedTheRun = true;
            gameObject.SetActive(false);
        }

        private void PlayerDie()
        {
            var currentTotalGold = PlayerPrefs.GetInt("TotalGold", 0);
            if (_playerGold != 0)
            {
                currentTotalGold += _playerGold - 1;
                PlayerPrefs.SetInt("TotalGold", currentTotalGold);
            }
            else
            {
                currentTotalGold += _playerGold;
                PlayerPrefs.SetInt("TotalGold", currentTotalGold);
            }
            
            EnemySpawnManager.Instance.gameObject.SetActive(false);

            if (_youDiedPanel != null)
            {
                _gameTimeText.gameObject.SetActive(false);
                _healthBar.gameObject.SetActive(false);
                _youDiedPanel.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}