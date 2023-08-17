using Containers;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class EnemyAI : MonoBehaviour
    {
        private Transform _tower;
        private NavMeshAgent _agent;
        private EnemyData _enemyData; // Reference to the enemy data scriptable object

        private void Awake()
        {
            _tower = FindObjectOfType<PlayerController>().transform;
            _agent = GetComponent<NavMeshAgent>();

            if (_agent != null)
            {
                _agent.speed = GetComponent<EnemyController>().enemyData.moveSpeed;
            }
        }

        private void Update()
        {
            MoveToTower();
        }

        private void MoveToTower()
        {
            _agent.SetDestination(_tower.position);
        }
    }
}