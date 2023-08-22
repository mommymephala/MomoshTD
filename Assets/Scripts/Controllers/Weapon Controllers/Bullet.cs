using Containers;
using Controllers.Enemy_Controllers;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return; // Don't collide with the turret that fired it
        
            GameObject hitObject = other.gameObject;
            if (hitObject.CompareTag("Enemy")) // Check if the collided object is an enemy
            {
                BulletHit(hitObject);
            }

            Destroy(gameObject); // Destroy the bullet after collision
        }

        private void BulletHit(GameObject enemy)
        {
            // Assuming your enemy has a script that handles damage
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController == null) return;
            enemyController.TakeDamage(weaponData.baseDamage);
        }
    }
}