using System;
using Containers;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BaseWeaponController : MonoBehaviour
    {
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected TowerData towerData;

        [SerializeField] protected Transform towerTransform;
        [SerializeField] protected Transform muzzleTransform;
        [SerializeField] protected float rotationSpeed;

        protected Vector3 TurretPosition;
        protected Vector3 TargetPosition;
        protected Vector3 MuzzleLocation;
        protected Transform TargetEnemy;

        protected virtual void Update()
        {
        }
        
        protected void RotateTurretTowardsEnemy()
        {
            TurretPosition = towerTransform.position;
            TargetPosition = TargetEnemy.position;
            MuzzleLocation = muzzleTransform.position;

            Quaternion targetRotation = Quaternion.LookRotation(TargetPosition - TurretPosition);
            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        
        public void SetTargetEnemy(Transform enemyTransform)
        {
            TargetEnemy = enemyTransform;
        }
    }
}