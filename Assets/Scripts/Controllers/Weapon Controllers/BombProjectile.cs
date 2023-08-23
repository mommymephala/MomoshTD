using Containers;
using Controllers.Enemy_Controllers;
using UnityEngine;

namespace Controllers.Weapon_Controllers
{
    public class BombProjectile : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float explosionDuration = 0.5f; 

        private bool _hasExploded;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasExploded || other.CompareTag("Player")) return;

            Explode();
            _hasExploded = true;
            Destroy(gameObject, explosionDuration);
        }

        private void Explode()
        {
            var colliders = Physics.OverlapSphere(transform.position, (weaponData.baseAoeRadius + (weaponData.baseAoeRadius * towerData.baseAoeRadiusModifier)), enemyLayer);
            foreach (Collider collider in colliders)
            {
                var enemyController = collider.GetComponent<EnemyController>();
                if (enemyController == null) continue;
                enemyController.TakeDamage(Mathf.RoundToInt(weaponData.baseDamage + (weaponData.baseDamage * towerData.baseDmgModifier)));
            }
        }
    }
}