using Containers;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BaseWeaponController : MonoBehaviour
    {
        [SerializeField] public WeaponData weaponData;
        [SerializeField] protected TowerData towerData;

        [SerializeField] protected Transform towerTransform;
        [SerializeField] protected Transform muzzleTransform;
        [SerializeField] protected float rotationSpeed;

        private Vector3 _turretPosition;
        private Vector3 _targetPosition;
        protected Vector3 MuzzleLocation;
        protected Transform TargetEnemy;

        protected virtual void Update()
        {
        }
        
        protected void RotateTurretTowardsEnemy()
        {
            _turretPosition = towerTransform.position;
            _targetPosition = TargetEnemy.position;
            MuzzleLocation = muzzleTransform.position;

            Quaternion targetRotation = Quaternion.LookRotation(_targetPosition - _turretPosition);
            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        
        public void SetTargetEnemy(Transform enemyTransform)
        {
            TargetEnemy = enemyTransform;
        }
    }
}