using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class TurretController : BaseWeaponController
    {
        private float _attackTimer; // Cooldown timer for the weapon

        protected override void Update()
        {
            _attackTimer += Time.deltaTime;
            if (TargetEnemy == null)
                return;
            
            RotateTurretTowardsEnemy();
            
            if (weaponData == null) return;
            if (!(_attackTimer >= weaponData.baseCooldown * towerData.baseCooldownModifier)) return;
            FireBullet((TargetEnemy.position - MuzzleLocation).normalized);
            _attackTimer = 0f;
        }

        private void FireBullet(Vector3 bulletDirection)
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, MuzzleLocation, Quaternion.identity);
            var bulletRigidbody = projectile.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = bulletDirection * weaponData.baseProjectileSpeed;
            }
        }
    }
}