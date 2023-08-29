using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class TurretController : BaseWeaponController
    {
        protected override void Update()
        {
            AttackTimer += Time.deltaTime;
            if (TargetEnemy == null)
                return;
            
            RotateTurretTowardsEnemy();
            
            if (weaponData == null) return;
            if (!(AttackTimer >= CurrentCooldown * currentCooldownModifier)) return;
            FireBullet((TargetEnemy.position - MuzzleLocation).normalized);
            AttackTimer = 0f;
        }

        private void FireBullet(Vector3 bulletDirection)
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, MuzzleLocation, Quaternion.identity);
            var bulletRigidbody = projectile.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = bulletDirection * (CurrentProjectileSpeed * currentProjectileSpeedModifier);
            }
        }
    }
}