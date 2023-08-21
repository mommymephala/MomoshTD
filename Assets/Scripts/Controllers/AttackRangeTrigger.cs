using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class AttackRangeTrigger : MonoBehaviour
    {
        [SerializeField] private List<TurretController> weaponControllers;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float detectionRadius = 10f;

        private Collider[] _cachedColliders;
        private const int BatchSize = 50;
        private const float UpdateInterval = 0.5f;
        private float _lastUpdate = -Mathf.Infinity;

        private void Start()
        {
            _cachedColliders = new Collider[10];
        }

        private void Update()
        {
            if (!(Time.time - _lastUpdate >= UpdateInterval)) return;
            _lastUpdate = Time.time;
            StartCoroutine(UpdateWeaponTargets());
        }

        private IEnumerator UpdateWeaponTargets()
        {
            var numColliders =
                Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, _cachedColliders, enemyLayer);

            foreach (TurretController weaponController in weaponControllers)
            {
                EnemyController closestEnemy = null;
                var closestDistance = Mathf.Infinity;

                for (var i = 0; i < numColliders; i += BatchSize)
                {
                    var batchEnd = Mathf.Min(i + BatchSize, numColliders);
                    var collidersBatch = new Collider[batchEnd - i];
                    Array.Copy(_cachedColliders, i, collidersBatch, 0, batchEnd - i);

                    EnemyController enemy = GetClosestEnemy(collidersBatch);
                    if (enemy != null)
                    {
                        var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            closestEnemy = enemy;
                        }
                    }

                    yield return null;
                }

                weaponController.SetTargetEnemy(closestEnemy != null ? closestEnemy.transform : null);
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
