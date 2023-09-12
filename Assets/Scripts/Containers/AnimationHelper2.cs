using Controllers.Weapon_Controllers;
using UnityEngine;

public class AnimationHelper2 : MonoBehaviour
{
    public TurretController turretController;

    public void StartFiring()
    {
        turretController.StartFiring();
    }
}