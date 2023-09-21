using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class AutoGunController : BaseWeaponController
    {
        [SerializeField] private Animator autoGunAnimator;
        private Vector3 _bulletDirection;
        
        protected override void Update()
        {
            AttackTimer += Time.deltaTime;
            if (TargetEnemy == null)
                return;
            RotateWeaponTowardsEnemy();
            
            if (weaponData == null) return;
            if (!(AttackTimer >= CurrentCooldown * currentCooldownModifier)) return;
            _bulletDirection = (TargetEnemy.position - MuzzleLocation).normalized;
            FireBullet();
            AttackTimer = 0f;
        }

        private void FireBullet()
        {
            autoGunAnimator.SetTrigger("IsShooting");
        }

        public void StartFiring()
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, MuzzleLocation, Quaternion.identity);
            var bulletRigidbody = projectile.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = _bulletDirection * (CurrentProjectileSpeed * currentProjectileSpeedModifier);
            }
        }
    }
}