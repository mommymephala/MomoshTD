using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Tower", menuName = "New Tower")]
    public class TowerData : ScriptableObject
    {
        //those will be used for passive upgrades and base calculations ALL OF THEM SHOULD BE CALCULATED AS %
        
        //weapon values
        public float baseDmgModifier;
        public float baseAoeRadiusModifier;
        public float baseProjectileSpeedModifier;
        //public float baseAoeDurationModifier;
        public float baseCooldownModifier;
        
        //player values
        public int maxHp;
        public float baseHpRegen;
    }
}
