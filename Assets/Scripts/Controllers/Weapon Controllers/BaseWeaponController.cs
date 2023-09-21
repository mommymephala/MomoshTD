using Containers;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BaseWeaponController : MonoBehaviour
    {
        [SerializeField] public WeaponData weaponData;
        [SerializeField] protected Transform towerTransform;
        [SerializeField] protected Transform muzzleTransform;
        [SerializeField] protected float rotationSpeed;
        
        private Vector3 _targetPosition;
        protected Transform TargetEnemy;
        
        private Vector3 _weaponPosition;
        protected Vector3 MuzzleLocation;
        
        protected float AttackTimer;
        protected float CurrentCooldown;
        protected float CurrentProjectileSpeed;
        public float currentCooldownModifier = 1;
        public float currentProjectileSpeedModifier = 1;
        public float damageModifier = 1;
        public float areaModifier = 1;

        protected virtual void Start()
        {
            CurrentCooldown = weaponData.baseCooldown;
            CurrentProjectileSpeed = weaponData.baseProjectileSpeed;
        }

        protected virtual void Update()
        {
        }
        
        protected void RotateWeaponTowardsEnemy()
        {
            if (TargetEnemy == null)
                return;

            _weaponPosition = towerTransform.position;
            _targetPosition = TargetEnemy.position;
            MuzzleLocation = muzzleTransform.position;

            Vector3 targetDirectionXZ = _targetPosition - _weaponPosition;
            targetDirectionXZ.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirectionXZ);

            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);
        }
        
        public void SetTargetEnemy(Transform enemyTransform)
        {
            TargetEnemy = enemyTransform;
        }
    }
}