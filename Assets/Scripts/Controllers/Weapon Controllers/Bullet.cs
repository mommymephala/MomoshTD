using UnityEngine;
using Containers;
using Controllers.Enemy_Controllers;

namespace Controllers.Weapon_Controllers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private BaseWeaponController weaponController;
        private float _currentDamage;
        private float _currentArea;

        private void Start()
        {
            _currentDamage = weaponData.baseDamage;
            _currentArea = weaponData.baseAoeRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;

            GameObject hitObject = other.gameObject;
            if (hitObject.CompareTag("Enemy"))
            {
                BulletHit(hitObject);
            }

            // Check if this bullet has an AOE effect
            if (_currentArea > 0)
            {
                ApplyAOE(hitObject.transform.position);
            }

            Destroy(gameObject);
        }

        private void BulletHit(GameObject enemy)
        {
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController == null) return;
            enemyController.TakeDamage(Mathf.RoundToInt(_currentDamage * weaponController.damageModifier));
        }

        private void ApplyAOE(Vector3 center)
        {
            // Find all enemies within the AOE radius and apply damage to them
            var colliders = Physics.OverlapSphere(center, _currentArea * weaponController.areaModifier);
            foreach (Collider bulletCollider in colliders)
            {
                if (!bulletCollider.CompareTag("Enemy")) continue;
                var enemyController = bulletCollider.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(Mathf.RoundToInt(_currentDamage * weaponController.damageModifier));
                }
            }
        }
    }
}
