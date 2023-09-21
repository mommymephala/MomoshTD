using UnityEngine;
using Containers;
using Controllers.Player_Controllers;

namespace Controllers.Enemy_Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Scaling Calculations")]
        [SerializeField] private float maxDamageScalingTime;
        [SerializeField] private float maxDamageScaleValue;
        
        //references
        public EnemyData enemyData;
        private int _currentHealth;
        private float _nextDamageTime;
        private PlayerController _playerController;
        
        [Header("Coin Values")]
        [SerializeField] private GameObject xpGemPrefab;
        [SerializeField] private GameObject goldPrefab;
        [SerializeField] private GameObject healthPickupPrefab;
        [SerializeField] private float healthDropChanceMultiplier;
        [SerializeField] private float minSpawnOffsetX;
        [SerializeField] private float maxSpawnOffsetX;
        [SerializeField] private float minZOffset;
        [SerializeField] private float maxZOffset;

        private void Awake()
        {
            _currentHealth = enemyData.baseHealth;
            _nextDamageTime = Time.time + enemyData.damageInterval;
            _playerController = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            CauseDamageOverTime();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void CauseDamageOverTime()
        {
            if (!(Time.time >= _nextDamageTime)) return;

            // Calculate damage scaling factor based on elapsed time
            var damageScalingFactor = Mathf.Lerp(1.0f, maxDamageScaleValue, Mathf.Clamp01(Time.time / maxDamageScalingTime));
            var scaledDamage = Mathf.RoundToInt(enemyData.baseDamage * damageScalingFactor);

            _nextDamageTime = Time.time + enemyData.damageInterval;
            if (!IsCollidingWithTower()) return;
            _playerController.TowerTakeDamage(scaledDamage);
        }

        private bool IsCollidingWithTower()
        {
            Vector3 checkPosition = transform.position;
            
            var isColliding = Physics.CheckSphere(checkPosition, enemyData.damageRadius, LayerMask.GetMask("Player"));
            return isColliding;
        }
        
        private void OnDrawGizmos()
        {
            Vector3 checkPosition = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checkPosition, enemyData.damageRadius);
        }

        public void Die()
        {
            SpawnCoins(transform.position);
            
            var healthDropChance = CalculateHealthDropChance();
            var randomValue = Random.value;
            if (randomValue < healthDropChance)
            {
                SpawnHealthPickup(transform.position);
            }

            GameObject vfx = Instantiate(enemyData.enemyDeathVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f);
            Destroy(gameObject);
        }
        
        private float CalculateHealthDropChance()
        {
            var healthDropChance = enemyData.healthDropChance;

            if (_playerController.currentHealth <= _playerController.maxCurrentHealth * 0.25f)
            {
                healthDropChance += healthDropChanceMultiplier;
            }

            return Mathf.Clamp01(healthDropChance);
        }

        private void SpawnHealthPickup(Vector3 spawnPosition)
        {
            Vector3 healthPickupSpawnPosition = spawnPosition + new Vector3(Random.Range(minSpawnOffsetX, maxSpawnOffsetX), 0.0f, Random.Range(minZOffset, maxZOffset));
            Instantiate(healthPickupPrefab, healthPickupSpawnPosition, Quaternion.identity);
        }

        private void SpawnCoins(Vector3 spawnPosition)
        {
            var randomXpGemAmount = Random.Range(enemyData.xpGemMinDropAmount, enemyData.xpGemMaxDropAmount + 1);
            for (var i = 0; i < randomXpGemAmount; i++)
            {
                Vector3 xpGemSpawnPosition = spawnPosition + new Vector3(Random.Range(minSpawnOffsetX, maxSpawnOffsetX),0.0f, Random.Range(minZOffset, maxZOffset));
                Instantiate(xpGemPrefab, xpGemSpawnPosition, Quaternion.identity);
            }
            
            if (!(Random.value <= enemyData.goldDropChance)) return;
            {
                var randomGoldAmount = Random.Range(enemyData.goldMinDropAmount, enemyData.goldMaxDropAmount + 1);
                for (var i = 0; i < randomGoldAmount; i++)
                {
                    Vector3 goldSpawnPosition = spawnPosition + new Vector3(Random.Range(minSpawnOffsetX, maxSpawnOffsetX), 0.0f, Random.Range(minZOffset, maxZOffset));
                    Instantiate(goldPrefab, goldSpawnPosition, Quaternion.identity);
                }
            }
        }
    }
}