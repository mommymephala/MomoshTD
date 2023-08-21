using UnityEngine;
using Controllers;
using Containers;

public class ExplodingBullet : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private LayerMask enemyLayer;
    private GameObject _owner; // The turret that fired the bullet, set when instantiated
    
    public void Initialize(GameObject owner)
    {
        _owner = owner;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner || other.CompareTag("Player")) return;
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        var colliders = Physics.OverlapSphere(transform.position, weaponData.effectRadius, enemyLayer);
        foreach (Collider collider in colliders)
        {
            var enemyController = collider.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(weaponData.damage);
            }
        }
    }
}