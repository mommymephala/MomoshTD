using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Tower", menuName = "New Tower")]
    public class TowerData : ScriptableObject
    {
        //those will be used for passive upgrades and base calculations
        public float baseDmgModifier;
        public float baseAreaModifier;
        public float baseRangeModifier;
        public float baseProjectileSpeedModifier;
        public float baseDurationModifier;
        public float baseCooldownModifier;
        public float baseLuckModifier;
        public int maxHp;
        public float baseHpRegen;
    }
}
