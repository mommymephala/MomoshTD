using UnityEngine;
using Containers;
// ReSharper disable Unity.PreferNonAllocApi

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TowerData towerData;
        [SerializeField] private LayerMask itemLayer;
        [SerializeField] private float collectionRadius = 5f;
        private int _currentHealth;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _currentHealth = towerData.maxHp;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CollectItemsAtClick();
            }
        }
        
        private Vector3 GetMouseClickPosition()
        {
            Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out RaycastHit hit) ? hit.point : Vector3.zero;
        }
        
        private void CollectItemsAtClick()
        {
            // Get the mouse click position in the game world
            Vector3 clickPosition = GetMouseClickPosition();

            // Collect items within the specified radius of the click position
            var colliders = Physics.OverlapSphere(clickPosition, collectionRadius, itemLayer);
            foreach (Collider itemCollider in colliders)
            {
                // Handle collecting the item (gem or gold)
                CollectItem(itemCollider.gameObject);
            }
        }

        private void CollectItem(Object item)
        {
            // Destroy the collected item GameObject
            Destroy(item);
        }

        public void TowerTakeDamage(int damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}