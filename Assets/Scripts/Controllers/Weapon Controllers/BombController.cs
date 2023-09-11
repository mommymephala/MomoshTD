using Unity.VisualScripting;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BombController : BaseWeaponController
    {
        [SerializeField] private Animator bazookaAnimator;
        private Vector3 _bombDirection;
        
        protected override void Update()
        {
            AttackTimer += Time.deltaTime;
            if (TargetEnemy == null)
                return;
            
            RotateWeaponTowardsEnemy();
            
            if (weaponData == null) return;
            if (!(AttackTimer >= CurrentCooldown * currentCooldownModifier)) return;
            _bombDirection = (TargetEnemy.position - MuzzleLocation).normalized;
            FireBomb();
            AttackTimer = 0f;
        }
        
        private void FireBomb()
        {
            bazookaAnimator.SetTrigger("IsShooting");   
        }
        
        public void StartFiringAnimation()
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, MuzzleLocation, Quaternion.identity);
            var bombRigidbody = projectile.GetComponent<Rigidbody>();
            if (bombRigidbody != null)
            {
                bombRigidbody.velocity = _bombDirection * (CurrentProjectileSpeed * currentProjectileSpeedModifier);
            }
        }
        
        /*public void StopFiringAnimation()
        {
            bazookaAnimator.SetBool("IsShooting", false);
        }*/
    }
}
