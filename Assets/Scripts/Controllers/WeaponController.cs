using UnityEngine;
using Containers;

namespace Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Transform towerMuzzleMeshTransform;
        [SerializeField] private LayerMask enemyLayer;
        private float _attackTimer;
        private Transform _targetEnemy;
        private Vector3 _turretPosition; // Cache turret position
        private Vector3 _targetPosition; // Cache target position
        
        private void Update()
        {
            AttackEnemies();
        }

        private void AttackEnemies()
        {
            _attackTimer += Time.deltaTime;

            if (!(_attackTimer >= weaponData.cooldown) || _targetEnemy == null) return;

            _turretPosition = towerMuzzleMeshTransform.position; // Update cached position
            _targetPosition = _targetEnemy.position; // Update cached position
            Vector3 turretToEnemy = _targetPosition - _turretPosition;

            if (!Physics.Raycast(_turretPosition, turretToEnemy, out RaycastHit hit, weaponData.effectArea,
                    enemyLayer)) return;
            if (enemyLayer != (enemyLayer | (1 << hit.collider.gameObject.layer))) return;

            // Set the LookAt rotation
            towerMuzzleMeshTransform.LookAt(_targetEnemy);

            var enemy = hit.collider.GetComponentInParent<EnemyController>();
            
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.damage);
            }
            
            _attackTimer = 0f;
        }
        
        // Called when an enemy enters the weapon's attack range
        public void SetTargetEnemy(Transform enemyTransform)
        {
            _targetEnemy = enemyTransform;
        }
    }
}
