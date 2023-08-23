using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BombController : BaseWeaponController
    {
        private float _attackTimer; // Cooldown timer for the weapon

        protected override void Update()
        {
            _attackTimer += Time.deltaTime;
            if (TargetEnemy == null)
                return;
            
            RotateTurretTowardsEnemy();
            
            if (weaponData == null) return;
            if (!(_attackTimer >= weaponData.baseCooldown + (weaponData.baseCooldown * towerData.baseCooldownModifier))) return;
            FireBomb((TargetEnemy.position - MuzzleLocation).normalized);
            _attackTimer = 0f;
        }
        
        private void FireBomb(Vector3 bombDirection)
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, MuzzleLocation, Quaternion.identity);
            var bombRigidbody = projectile.GetComponent<Rigidbody>();
            if (bombRigidbody != null)
            {
                bombRigidbody.velocity = bombDirection * (weaponData.baseProjectileSpeed + (weaponData.baseProjectileSpeed * towerData.baseProjectileSpeedModifier));
            }
        }
    }
}
