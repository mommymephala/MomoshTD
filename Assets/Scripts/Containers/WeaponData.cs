using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "New Weapon")]
    public class WeaponData : ScriptableObject
    {
        //those will be used for base calculations
        public GameObject projectilePrefab;
        public int baseDamage;
        public float baseProjectileSpeed;
        public float baseCooldown;
        public float baseAoeRadius;
    }
}
