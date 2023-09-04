using System.Diagnostics.CodeAnalysis;
using Containers;
using Controllers.Player_Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Enemy_Controllers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public static EnemySpawnManager Instance;
        private PlayerController _playerController;
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private EnemyData bigEnemyData;
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
        private float _bossSpawnTime;
        private bool _hasSpawnedBoss;

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
        }

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _spawnPoint = transform;
            _nextSpawnTime = Time.time + Random.Range(spawnFrequencyMin, spawnFrequencyMax);
            _bossSpawnTime = 60f;
            _hasSpawnedBoss = false;
        }

        private void Update()
        {
            if (!_hasSpawnedBoss && _playerController.gameTime >= _bossSpawnTime)
            {
                SpawnBoss();
                _hasSpawnedBoss = true;
            }
                        
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
        public void SpawnBigEnemy(float bigEnemySpawnChance)
        {
            Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
            randomSpawnPosition.y = _spawnPoint.position.y;

            Vector3 towerPosition = towerLocation.transform.position;
            
            Vector3 directionToTower = towerPosition - randomSpawnPosition;
            if (directionToTower.magnitude < minDistanceFromTower)
            {
                randomSpawnPosition = towerPosition + directionToTower.normalized * minDistanceFromTower;
            }

            if (Physics.OverlapSphere(randomSpawnPosition, 1f, obstacleLayer).Length > 0) return;
            if (Random.value <= bigEnemySpawnChance)
            {
                Instantiate(bigEnemyData.enemyPrefab, randomSpawnPosition, Quaternion.identity);
            }
        }
        
        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        private void SpawnBoss()
        {
            Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
            randomSpawnPosition.y = _spawnPoint.position.y;

            Vector3 towerPosition = towerLocation.transform.position;
            
            Vector3 directionToTower = towerPosition - randomSpawnPosition;
            if (directionToTower.magnitude < minDistanceFromTower)
            {
                randomSpawnPosition = towerPosition + directionToTower.normalized * minDistanceFromTower;
            }

            if (Physics.OverlapSphere(randomSpawnPosition, 1f, obstacleLayer).Length > 0) return;
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