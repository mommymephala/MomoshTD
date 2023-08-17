using UnityEngine;
using Containers;

namespace Controllers
{
    public class BaseWeaponController : MonoBehaviour
    {
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected TowerData towerData;
        [SerializeField] protected Transform towerTransform; // Parent transform containing both tower and muzzle
        [SerializeField] protected LayerMask enemyLayer;
        [SerializeField] protected float rotationSpeed;
        protected float _attackTimer;
        protected Transform _targetEnemy;
        protected Vector3 _turretPosition; // Cache turret position
        protected Vector3 _targetPosition; // Cache target position

        protected virtual void Update()
        {
            AttackEnemies();
        }

        protected virtual void AttackEnemies()
        {
            _attackTimer += Time.deltaTime;

            if (!(_attackTimer >= CalculateCooldown()) || _targetEnemy == null) return;

            _turretPosition = towerTransform.position; // Update cached position
            _targetPosition = _targetEnemy.position; // Update cached position
            Vector3 turretToEnemy = _targetPosition - _turretPosition;

            if (!Physics.Raycast(_turretPosition, turretToEnemy, out RaycastHit hit, CalculateEffectArea(), enemyLayer)) return;
            if (enemyLayer != (enemyLayer | (1 << hit.collider.gameObject.layer))) return;

            Debug.DrawRay(_turretPosition, turretToEnemy, Color.blue);

            // Calculate the rotation needed to LookAt the target
            Quaternion targetRotation = Quaternion.LookRotation(turretToEnemy);
            // Smoothly interpolate between the current rotation and the target rotation
            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            var enemy = hit.collider.GetComponentInParent<EnemyController>();

            if (enemy != null)
            {
                enemy.TakeDamage(CalculateDamage());
            }

            _attackTimer = 0f;
        }

        protected float CalculateCooldown()
        {
            return weaponData.cooldown * towerData.baseCooldownModifier;
        }

        protected int CalculateDamage()
        {
            return Mathf.RoundToInt(weaponData.damage * towerData.baseDmgModifier);
        }

        protected float CalculateEffectArea()
        {
            return weaponData.effectArea * towerData.baseAreaModifier;
        }

        public void SetTargetEnemy(Transform enemyTransform)
        {
            _targetEnemy = enemyTransform;
        }
    }
}
