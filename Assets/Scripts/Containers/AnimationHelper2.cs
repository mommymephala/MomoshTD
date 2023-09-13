using Controllers.Weapon_Controllers;
using UnityEngine;

namespace Containers
{
    public class AnimationHelper2 : MonoBehaviour
    {
        public TurretController turretController;
        public void StartFiring()
        {
            turretController.StartFiring();
        }
    }
}