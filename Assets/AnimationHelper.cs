using Controllers.Weapon_Controllers;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public BombController bombController;

    public void StartFiringAnimation()
    {
        bombController.StartFiringAnimation();
    }
}
