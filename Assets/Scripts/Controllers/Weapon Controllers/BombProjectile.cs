using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Containers;
using Controllers.Enemy_Controllers;

namespace Controllers.Weapon_Controllers
{
    public class BombProjectile : MonoBehaviour
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private TowerData towerData;
        [SerializeField] private BaseWeaponController weaponController;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float explosionDuration = 0.5f;
        [SerializeField] private GameObject explosionVFX;
        private float _currentDamage;
        private float _currentArea;

        private bool _hasExploded;

        private void Start()
        {
            _currentDamage = weaponData.baseDamage + (weaponData.baseDamage * towerData.baseDmgModifier);
            _currentArea = weaponData.baseAoeRadius + (weaponData.baseAoeRadius * towerData.baseAoeRadiusModifier);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasExploded || other.CompareTag("Player")) return;

            Explode();
            GameObject vfxClone = Instantiate(explosionVFX, transform.position,Quaternion.identity);
            _hasExploded = true;
            Destroy(vfxClone, 1.5f);
            Destroy(gameObject, explosionDuration);
        }

        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        private void Explode()
        {
            var colliders = Physics.OverlapSphere(transform.position, (_currentArea * weaponController.areaModifier), enemyLayer);
            foreach (Collider enemyCollider in colliders)
            {
                var enemyController = enemyCollider.GetComponent<EnemyController>();
                if (enemyController == null) continue;
                enemyController.TakeDamage(Mathf.RoundToInt(_currentDamage * weaponController.damageModifier));
            }
        }
    }
}