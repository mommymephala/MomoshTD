using Containers;
using UnityEngine;
// ReSharper disable Unity.PreferNonAllocApi

namespace Controllers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnFrequencyMin = 2f;
        [SerializeField] private float spawnFrequencyMax = 5f;
        [SerializeField] private int minEnemiesPerSpawn = 5;
        [SerializeField] private int maxEnemiesPerSpawn = 10;
        [SerializeField] private float spawnRadius = 5f;
        [SerializeField] private LayerMask obstacleLayer;

        private Transform _spawnPoint;
        private float _nextSpawnTime;
        
        private void Start()
        {
            _spawnPoint = transform;
            _nextSpawnTime = Time.time + Random.Range(spawnFrequencyMin, spawnFrequencyMax);
        }

        private void Update()
        {
            // Check if it's time to spawn enemies
            if (!(Time.time >= _nextSpawnTime)) return;
            var numEnemiesToSpawn = Random.Range(minEnemiesPerSpawn, maxEnemiesPerSpawn + 1);

            for (var i = 0; i < numEnemiesToSpawn; i++)
            {
                SpawnEnemy();
            }

            // Calculate the time for the next spawn
            _nextSpawnTime = Time.time + Random.Range(spawnFrequencyMin, spawnFrequencyMax);
        }

        private void SpawnEnemy()
        {
            // Find a random position within the spawn radius
            Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
            randomSpawnPosition.y = _spawnPoint.position.y; // Keep the same y-coordinate as spawn point

            // Check for obstacles using overlap sphere
            if (Physics.OverlapSphere(randomSpawnPosition, 1f, obstacleLayer).Length <= 0)
                Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
