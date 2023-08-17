using UnityEngine;
using Containers;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] public EnemyData enemyData;
        [SerializeField] private GameObject gemPrefab;
        [SerializeField] private GameObject goldPrefab;
        private PlayerController _playerController;
        private int _currentHealth;
        private float _nextDamageTime;

        private void Start()
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
            _nextDamageTime = Time.time + enemyData.damageInterval;
            if (IsCollidingWithTower())
            {
                _playerController.TowerTakeDamage(enemyData.damage);
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

        private void Die()
        {
            // Spawn gems and gold at the enemy's position
            SpawnGemsAndGold(transform.position);
            // Destroy the enemy GameObject
            Destroy(gameObject);
        }

        private void SpawnGemsAndGold(Vector3 spawnPosition)
        {
            // Offset the spawn positions slightly
            Vector3 gemSpawnPosition = spawnPosition + new Vector3(0.0f, 0.1f, 0.0f);
            Vector3 goldSpawnPosition = spawnPosition + new Vector3(0.0f, 0.2f, 0.0f);

            GameObject gem = Instantiate(gemPrefab, gemSpawnPosition, Quaternion.identity);
            GameObject gold = Instantiate(goldPrefab, goldSpawnPosition, Quaternion.identity);
        }

    }
}
