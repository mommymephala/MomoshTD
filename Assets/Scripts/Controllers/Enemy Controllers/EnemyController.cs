using Containers;
using Controllers.Player_Controllers;
using UnityEngine;

namespace Controllers.Enemy_Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] public EnemyData enemyData;
        [SerializeField] private float maxDamageScalingTime;
        [SerializeField] private float maxDamageScaleValue; // Represents the duration in seconds over which you want the scaling to occur.
        private int _currentHealth;
        private float _nextDamageTime;
        private PlayerController _playerController;
        
        //coin variables
        [SerializeField] private GameObject xpGemPrefab;
        [SerializeField] private GameObject goldPrefab;
        [SerializeField] private float minSpawnOffsetX;
        [SerializeField] private float maxSpawnOffsetX;
        [SerializeField] private float minZOffset;
        [SerializeField] private float maxZOffset;

        private void Awake()
        {
            _currentHealth = enemyData.health;
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
            var scaledDamage = Mathf.RoundToInt(enemyData.damage * damageScalingFactor);

            _nextDamageTime = Time.time + enemyData.damageInterval;
            if (IsCollidingWithTower())
            {
                _playerController.TowerTakeDamage(scaledDamage);
            }
        }

        private bool IsCollidingWithTower()
        {
            // Calculate the position where you want to check for collision (e.g., the center of the enemy)
            Vector3 checkPosition = transform.position;
            
            // Check if there are any colliders within the specified radius that belong to the Tower layer
            var isColliding = Physics.CheckSphere(checkPosition, enemyData.damageRadius, LayerMask.GetMask("Player"));
            return isColliding;
        }
        
        private void OnDrawGizmos()
        {
            Vector3 checkPosition = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checkPosition, enemyData.damageRadius);
        }

        private void Die()
        {
            SpawnCoins(transform.position);
            Destroy(gameObject);
        }

        private void SpawnCoins(Vector3 spawnPosition)
        {
            // Instantiate xp coins based on randomized amount and drop chance
            var randomXpGemAmount = Random.Range(enemyData.xpGemMinDropAmount, enemyData.xpGemMaxDropAmount + 1);
            for (var i = 0; i < randomXpGemAmount; i++)
            {
                Vector3 xpGemSpawnPosition = spawnPosition + new Vector3(Random.Range(minSpawnOffsetX, maxSpawnOffsetX),0.0f, Random.Range(minZOffset, maxZOffset));
                Instantiate(xpGemPrefab, xpGemSpawnPosition, Quaternion.identity);
            }
            
            // Instantiate gold coins based on randomized amount and drop chance
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
