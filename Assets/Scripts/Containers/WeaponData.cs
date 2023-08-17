using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "New Weapon")]
    public class WeaponData : ScriptableObject
    {
        //those will be used for passive upgrades and base calculations
        public int damage;
        public float projectileSpeed;
        public float cooldown;
        public float effectDuration;
        public float effectArea;
    }
}
