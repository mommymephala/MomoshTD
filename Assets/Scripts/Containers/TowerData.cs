using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Tower", menuName = "New Tower")]
    public class TowerData : ScriptableObject
    {
        //those will be used for passive upgrades and base calculations
        
        //weapon values
        public float baseDmgModifier;
        public float baseAreaModifier;
        public float baseRangeModifier;
        public float baseProjectileSpeedModifier;
        public float baseDurationModifier;
        public float baseCooldownModifier;
        
        //player values
        public int maxHp;
        public float baseHpRegen;
    }
}
