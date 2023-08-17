using UnityEngine;

namespace Controllers
{
    public class BombWeaponController : BaseWeaponController
    {
        [SerializeField] private GameObject bombPrefab; // Prefab of the bomb to be thrown
        [SerializeField] private float throwForce; // Force at which the bomb is thrown

        // Override the AttackEnemies method with bomb-throwing behavior
        protected override void AttackEnemies()
        {
            _attackTimer += Time.deltaTime;
            
            if (!(_attackTimer >= CalculateCooldown()) || _targetEnemy == null) return;

            // Calculate throw direction
            Vector3 throwDirection = (_targetEnemy.position - _turretPosition).normalized;
            Debug.DrawRay(_turretPosition, throwDirection, Color.green);

            // Instantiate and throw the bomb
            GameObject bomb = Instantiate(bombPrefab, towerTransform.position, Quaternion.identity);
            var bombRigidbody = bomb.GetComponent<Rigidbody>();

            if (bombRigidbody != null)
            {
                bombRigidbody.velocity = throwDirection * throwForce;
                Debug.Log("Bomb thrown with velocity: " + bombRigidbody.velocity);
            }
            else
            {
                Debug.Log("Bomb rigidbody is null.");
            }

            _attackTimer = 0f;
        }
    }
}