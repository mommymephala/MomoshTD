using UnityEngine;
using Containers;

namespace Controllers
{
    public class TurretController : MonoBehaviour
    {
        [SerializeField] private WeaponData demoWeaponData;
        [SerializeField] private WeaponData explodingWeaponData;
        [SerializeField] private TowerData towerData;
        [SerializeField] private Transform towerTransform;
        [SerializeField] private Transform muzzleTransform;
        [SerializeField] private float rotationSpeed;

        private Transform _targetEnemy;
        private Vector3 _turretPosition;
        private Vector3 _targetPosition;
        private Vector3 _muzzleLocation;

        private float _demoAttackTimer; // Cooldown timer for demo weapon
        private float _explodingAttackTimer; // Cooldown timer for exploding weapon

        private void Update()
        {
            if (_targetEnemy == null)
                return;

            _demoAttackTimer += Time.deltaTime;
            _explodingAttackTimer += Time.deltaTime;

            if (_targetEnemy != null && demoWeaponData != null && _demoAttackTimer >= demoWeaponData.cooldown * towerData.baseCooldownModifier)
            {
                AttackWithRegularWeapon();
                _demoAttackTimer = 0f;
            }

            if (_targetEnemy != null && explodingWeaponData != null && _explodingAttackTimer >= explodingWeaponData.cooldown * towerData.baseCooldownModifier)
            {
                AttackWithExplodingWeapon();
                _explodingAttackTimer = 0f;
            }
        }

        private void AttackWithRegularWeapon()
        {
            _turretPosition = towerTransform.position; // Update cached position
            _targetPosition = _targetEnemy.position; // Update cached position
            _muzzleLocation = muzzleTransform.position;
            Vector3 turretToEnemy = _targetPosition - _turretPosition;

            Quaternion targetRotation = Quaternion.LookRotation(turretToEnemy);
            towerTransform.rotation = Quaternion.Lerp(towerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Calculate bullet's direction and speed
            Vector3 bulletDirection = turretToEnemy.normalized;
            GameObject bullet = Instantiate(demoWeaponData.projectilePrefab, _muzzleLocation, Quaternion.identity);
            var bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(gameObject); // Pass the turret gameObject as the owner
            }

            var bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = bulletDirection * demoWeaponData.projectileSpeed;
            }
        }

        private void AttackWithExplodingWeapon()
        {
            // Calculate bullet's direction and speed
            Vector3 turretToEnemy = _targetPosition - _turretPosition;
            Vector3 bulletDirection = turretToEnemy.normalized;

            _muzzleLocation = muzzleTransform.position;
            GameObject explodingBullet = Instantiate(explodingWeaponData.projectilePrefab, _muzzleLocation, Quaternion.identity);
            var explodingBulletScript = explodingBullet.GetComponent<ExplodingBullet>();
            if (explodingBulletScript != null)
            {
                explodingBulletScript.Initialize(gameObject); // Pass the turret gameObject as the owner
            }

            var explodingBulletRigidbody = explodingBullet.GetComponent<Rigidbody>();
            if (explodingBulletRigidbody != null)
            {
                explodingBulletRigidbody.velocity = bulletDirection * explodingWeaponData.projectileSpeed;
            }
        }

        public void SetTargetEnemy(Transform enemyTransform)
        {
            _targetEnemy = enemyTransform;
        }
    }
}
