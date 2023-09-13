using Controllers.Weapon_Controllers;
using UnityEngine;

namespace Containers
{
    public class AnimationHelper : MonoBehaviour
    {
        public BombController bombController;

        public void StartFiringAnimation()
        {
            bombController.StartFiringAnimation();
        }
    }
}
