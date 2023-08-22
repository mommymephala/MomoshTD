using Controllers.Player_Controllers;
using UnityEngine;

// ReSharper disable Unity.PreferNonAllocApi

namespace Controllers.Enemy_Controllers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnFrequencyMin;
        [SerializeField] private float spawnFrequencyMax;
        [SerializeField] private int minEnemiesPerSpawn;
        [SerializeField] private int maxEnemiesPerSpawn;
        [SerializeField] private float maxEnemiesScalingTime; // Represents the duration in seconds over which you want the scaling to occur.
        [SerializeField] private float spawnRadius;
        [SerializeField] private float minDistanceFromTower;
        [SerializeField] private LayerMask obstacleLayer;
        private Transform _spawnPoint;
        private float _nextSpawnTime;
        
        private void Awake()
        {
            _spawnPoint = transform;
            _nextSpawnTime = Time.time + Random.Range(spawnFrequencyMin, spawnFrequencyMax);
        }

        private void Update()
        {
            // Check if it's time to spawn enemies
            if (!(Time.time >= _nextSpawnTime)) return;
    
            var numEnemiesToSpawn = CalculateNumEnemiesToSpawn();

            for (var i = 0; i < numEnemiesToSpawn; i++)
            {
                SpawnEnemy();
            }

            // Calculate the time for the next spawn
            _nextSpawnTime = Time.time + Random.Range(spawnFrequencyMin, spawnFrequencyMax);
        }

        
        private int CalculateNumEnemiesToSpawn()
        {
            // Implement the logic to adjust the number of enemies based on time passed
            // For example, increase the maxEnemiesPerSpawn gradually as time passes
            var numEnemies = Mathf.RoundToInt(Mathf.Lerp(minEnemiesPerSpawn, maxEnemiesPerSpawn, Mathf.Clamp01(Time.time / maxEnemiesScalingTime)));
            return numEnemies;
        }

        private void SpawnEnemy()
        {
            // Find a random position within the spawn radius
            Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
            randomSpawnPosition.y = _spawnPoint.position.y; // Keep the same y-coordinate as spawn point

            // Calculate the position of the player tower
            Vector3 towerPosition = FindObjectOfType<PlayerController>().transform.position;

            // Ensure the randomSpawnPosition is at least minDistanceFromTower away from the tower
            Vector3 directionToTower = towerPosition - randomSpawnPosition;
            if (directionToTower.magnitude < minDistanceFromTower)
            {
                randomSpawnPosition = towerPosition + directionToTower.normalized * minDistanceFromTower;
            }

            // Check for obstacles using overlap sphere
            if (Physics.OverlapSphere(randomSpawnPosition, 1f, obstacleLayer).Length <= 0)
                Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);

            Vector3 towerPosition = FindObjectOfType<PlayerController>().transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(towerPosition, minDistanceFromTower);
        }
    }
}
