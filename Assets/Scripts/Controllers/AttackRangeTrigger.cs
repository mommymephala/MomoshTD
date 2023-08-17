using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class AttackRangeTrigger : MonoBehaviour
    {
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float detectionRadius = 10f;

        private Collider[] _cachedColliders;
        private const int BatchSize = 50; //Increasing this would improve performance(?) but can cause less accurate target selection
        private const float UpdateInterval = 0.5f; // Increasing this would increase gameplay responsiveness but in cost of performance
        private float _lastUpdate = -Mathf.Infinity; // Initialize to a negative value to trigger the first update immediately

        private void Start()
        {
            _cachedColliders = new Collider[10];
        }

        private void Update()
        {
            if (!(Time.time - _lastUpdate >= UpdateInterval)) return;
            _lastUpdate = Time.time;
            StartCoroutine(UpdateWeaponTargetBatched());
        }

        private IEnumerator UpdateWeaponTargetBatched()
        {
            var numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, _cachedColliders, enemyLayer);

            for (var i = 0; i < numColliders; i += BatchSize)
            {
                var batchEnd = Mathf.Min(i + BatchSize, numColliders);
                var collidersBatch = new Collider[batchEnd - i];
                System.Array.Copy(_cachedColliders, i, collidersBatch, 0, batchEnd - i);
                
                EnemyController closestEnemy = GetClosestEnemy(collidersBatch);
                weaponController.SetTargetEnemy(closestEnemy != null ? closestEnemy.transform : null);

                yield return null;
            }
        }

        private EnemyController GetClosestEnemy(IEnumerable<Collider> colliders)
        {
            EnemyController closestEnemy = null;
            var closestDistance = Mathf.Infinity;

            foreach (Collider enemyCollider in colliders)
            {
                var enemyController = enemyCollider.GetComponentInParent<EnemyController>();
                if (enemyController == null) continue;
                var distanceToEnemy = Vector3.Distance(transform.position, enemyController.transform.position);
                if (!(distanceToEnemy < closestDistance)) continue;
                closestDistance = distanceToEnemy;
                closestEnemy = enemyController;
            }

            return closestEnemy;
        }
    }
}
