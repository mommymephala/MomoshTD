using Containers;
using Controllers.Enemy_Controllers;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BombProjectile : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private LayerMask enemyLayer;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;
            
            Explode();
            Destroy(gameObject);
        }

        private void Explode()
        {
            var colliders = Physics.OverlapSphere(transform.position, weaponData.baseEffectRadius, enemyLayer);
            foreach (Collider collider in colliders)
            {
                var enemyController = collider.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(weaponData.baseDamage);
                }
            }
        }
    }
}