using System.Diagnostics.CodeAnalysis;
using Containers;
using UnityEngine;

namespace Controllers.Enemy_Controllers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public static EnemySpawnManager Instance;
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private EnemyData bossData;
        [SerializeField] private GameObject towerLocation;
        
        [SerializeField] private float spawnFrequencyMin;
        [SerializeField] private float spawnFrequencyMax;
        
        [SerializeField] private int minEnemiesPerSpawn;
        [SerializeField] private int maxEnemiesPerSpawn;
        
        [SerializeField] private float maxEnemiesScalingTime;
        
        [SerializeField] private float spawnRadius;
        [SerializeField] private float minDistanceFromTower;
        
        [SerializeField] private LayerMask obstacleLayer;
        private Transform _spawnPoint;
        private float _nextSpawnTime;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            _spawnPoint = transform;
            _nextSpawnTime = Time.time + Random.Range(spawnFrequencyMin, spawnFrequencyMax);
        }

        private void Update()
        {
            if (!(Time.time >= _nextSpawnTime)) return;
            var numEnemiesToSpawn = CalculateNumEnemiesToSpawn();

            for (var i = 0; i < numEnemiesToSpawn; i++)
            {
                SpawnEnemy();
            }

            _nextSpawnTime = Time.time + Random.Range(spawnFrequencyMin, spawnFrequencyMax);
        }

        private int CalculateNumEnemiesToSpawn()
        {
            var numEnemies = Mathf.RoundToInt(Mathf.Lerp(minEnemiesPerSpawn, maxEnemiesPerSpawn, Mathf.Clamp01(Time.time / maxEnemiesScalingTime)));
            return numEnemies;
        }

        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        private void SpawnEnemy()
        {
            Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
            randomSpawnPosition.y = _spawnPoint.position.y;

            Vector3 towerPosition = towerLocation.transform.position;

            Vector3 directionToTower = towerPosition - randomSpawnPosition;
            if (directionToTower.magnitude < minDistanceFromTower)
            {
                randomSpawnPosition = towerPosition + directionToTower.normalized * minDistanceFromTower;
            }

            if (Physics.OverlapSphere(randomSpawnPosition, 1f, obstacleLayer).Length <= 0)
            {
                Instantiate(enemyData.enemyPrefab, randomSpawnPosition, Quaternion.identity);
            }
        }
        
        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        public void SpawnBoss(float bossSpawnChance)
        {
            // Calculate a random position within the spawn radius
            Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
            randomSpawnPosition.y = _spawnPoint.position.y;

            // Make sure the boss spawns a minimum distance away from the tower
            Vector3 towerPosition = towerLocation.transform.position;
            Vector3 directionToTower = towerPosition - randomSpawnPosition;
            if (directionToTower.magnitude < minDistanceFromTower)
            {
                randomSpawnPosition = towerPosition + directionToTower.normalized * minDistanceFromTower;
            }

            if (Physics.OverlapSphere(randomSpawnPosition, 1f, obstacleLayer).Length > 0) return;
            if (Random.value <= bossSpawnChance)
            {
                Instantiate(bossData.enemyPrefab, randomSpawnPosition, Quaternion.identity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);

            if (towerLocation == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(towerLocation.transform.position, minDistanceFromTower);
        }
    }
}