using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "New Enemy")]    
    public class EnemyData : ScriptableObject
    {
        //those will be used for passive upgrades and base calculations
        public GameObject enemyPrefab;
        public int health;
        public int damage;
        public int xpGemMinDropAmount;
        public int xpGemMaxDropAmount;
        public int goldMinDropAmount;
        public int goldMaxDropAmount;
        public float goldDropChance;
        public float moveSpeed;
        public float damageInterval;
        public float damageRadius;
    }
}
