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
        public int xpGemDropAmount;
        public int goldDropAmount;
        public int moveSpeed;
        public float damageInterval;
        public float damageRadius;
    }
}
