using Containers;
using UnityEngine;

namespace Controllers.Enemy_Controllers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private Transform towerTransform;
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

        private void SpawnEnemy()
        {
            Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
            randomSpawnPosition.y = _spawnPoint.position.y;

            Vector3 towerPosition = towerTransform.position;

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);

            if (towerTransform == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(towerTransform.position, minDistanceFromTower);
        }
    }
}