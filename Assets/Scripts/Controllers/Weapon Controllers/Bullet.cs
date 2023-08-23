using Containers;
using Controllers.Enemy_Controllers;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private TowerData towerData;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;
        
            GameObject hitObject = other.gameObject;
            if (hitObject.CompareTag("Enemy"))
            {
                BulletHit(hitObject);
            }

            Destroy(gameObject);
        }

        private void BulletHit(GameObject enemy)
        {
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController == null) return;
            enemyController.TakeDamage(Mathf.RoundToInt(weaponData.baseDamage + (weaponData.baseDamage * towerData.baseDmgModifier)));
        }
    }
}