using Containers;
using Controllers.Enemy_Controllers;
using UnityEngine;

namespace Controllers.Player_Controllers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        private GameObject _owner; // The turret that fired the bullet, set when instantiated

        public void Initialize(GameObject owner)
        {
            _owner = owner;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner || other.CompareTag("Player")) return; // Don't collide with the turret that fired it
        
            GameObject hitObject = other.gameObject;
            if (hitObject.CompareTag("Enemy")) // Check if the collided object is an enemy
            {
                BulletHitEnemy(hitObject);
            }

            Destroy(gameObject); // Destroy the bullet after collision
        }

        private void BulletHitEnemy(GameObject enemy)
        {
            // Assuming your enemy has a script that handles damage
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController == null) return;
            enemyController.TakeDamage(weaponData.damage);
        }
    }
}