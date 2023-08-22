using Containers;
using Controllers.Managers;
using UnityEngine;

namespace Controllers.Player_Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask coinLayer;
        [SerializeField] private float collectionRadius;
        private float _currentHealth;
        private float _playerXp;
        private int _playerGold;
        private Camera _camera;
        private float _nextHpRegenTime;
        private float _bonusHpRegen;

        private void Awake()
        {
            _camera = Camera.main;
            _currentHealth = towerData.maxHp;
            _nextHpRegenTime = Time.time + 1.0f;
            _bonusHpRegen = towerData.baseHpRegen;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CollectCoinsAtClick();
            }

            // Check for HP regeneration
            if (_currentHealth >= towerData.maxHp || !(Time.time >= _nextHpRegenTime)) return;
            RegenerateHp();
            _nextHpRegenTime = Time.time + 1.0f;
        }

        private void RegenerateHp()
        {
            var hpToRegen = Mathf.RoundToInt(towerData.baseHpRegen + _bonusHpRegen);
            if (_currentHealth < towerData.maxHp)
            {
                _currentHealth = Mathf.Min(_currentHealth + hpToRegen, towerData.maxHp);
            }
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
            // ReSharper disable once Unity.PreferNonAllocApi
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
                    var goldAmount = GoldCoinController.GetGoldAmount();
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