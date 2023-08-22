using System.Collections.Generic;
using UnityEngine;
using Containers;
using Controllers.Managers;

namespace Controllers.Player_Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask coinLayer;
        
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

            // Check for HP regeneration
            if (!(_currentHealth < _maxCurrentHealth) || !(Time.time >= _nextHpRegenTime)) return;
            RegenerateHp();
            _nextHpRegenTime = Time.time + HpRegenInterval;
            
            CheckForLevelUp();
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
            var leveledUp = false; // Initialize the flag to indicate if a level-up occurred

            // Check for level-up
            while (_currentLevel < _xpLevelThresholds.Count && _playerXp >= _xpLevelThresholds[_currentLevel - 1])
            {
                _currentLevel++;
                Debug.Log("Level Up! Current Level: " + _currentLevel);
                leveledUp = true; // Set the flag to true since a level-up occurred
            }

            return leveledUp; // Return the flag indicating whether a level-up occurred
        }

        private void OnLevelUp()
        {
            if(CheckForLevelUp());
            {
                //do level up logic
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
            // Get the mouse click position in the game world
            Vector3 clickPosition = GetMouseClickPosition();

            // Collect items within the specified radius of the click position
            var colliders = Physics.OverlapSphere(clickPosition, collectionRadius, coinLayer);
            foreach (Collider coinCollider in colliders)
            {
                // Handle collecting the item (gem or gold)
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
            // Add death-related logic here
            Destroy(gameObject);
        }
    }
}
