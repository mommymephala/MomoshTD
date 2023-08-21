using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "New Weapon")]
    public class WeaponData : ScriptableObject
    {
        //those will be used for base calculations
        public GameObject projectilePrefab;
        public int damage;
        public float projectileSpeed;
        public float cooldown;
        public float effectDuration;
        public float effectRadius;
        public float maxDistance;
    }
}
