using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BombController : BaseWeaponController
    {
        protected override void Update()
        {
            AttackTimer += Time.deltaTime;
            if (TargetEnemy == null)
                return;
            
            RotateTurretTowardsEnemy();
            
            if (weaponData == null) return;
            if (!(AttackTimer >= CurrentCooldown * currentCooldownModifier)) return;
            FireBomb((TargetEnemy.position - MuzzleLocation).normalized);
            AttackTimer = 0f;
        }
        
        private void FireBomb(Vector3 bombDirection)
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, MuzzleLocation, Quaternion.identity);
            var bombRigidbody = projectile.GetComponent<Rigidbody>();
            if (bombRigidbody != null)
            {
                bombRigidbody.velocity = bombDirection * (CurrentProjectileSpeed * currentProjectileSpeedModifier);
            }
        }
    }
}
