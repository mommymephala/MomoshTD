using Controllers.Player_Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers.Enemy_Controllers
{
    public class EnemyAI : MonoBehaviour
    {
        private Transform _tower;
        private NavMeshAgent _agent;

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
            if (_tower != null)
            {
                MoveToTower();
            }
            else
            {
                _agent.speed = 0;
            }
        }

        private void MoveToTower()
        {
            _agent.SetDestination(_tower.position);
        }
    }
}