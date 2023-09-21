using Controllers.Weapon_Controllers;
using UnityEngine;

namespace Containers
{
    public class AnimationHelper2 : MonoBehaviour
    {
        public AutoGunController autoGunController;
        public void StartFiring()
        {
            autoGunController.StartFiring();
        }
    }
}