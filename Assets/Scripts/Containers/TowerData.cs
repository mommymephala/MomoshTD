using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Tower", menuName = "New Tower")]
    public class TowerData : ScriptableObject
    {
        //those will be used for passive upgrades and base calculations
        public float maxDmgModifier;
        public float maxAreaModifier;
        public float maxProjectileSpeedModifier;
        public float maxDurationModifier;
        public float maxCooldownModifier;
        public float maxLuckModifier;
        public int maxHp;
        public float maxHpRecovery;
    }
}
