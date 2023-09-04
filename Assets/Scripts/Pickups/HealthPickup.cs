using UnityEngine;

namespace Pickups
{
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] private float healPercentage = 0.05f;

        public int GetHealAmount(float maxPlayerHealth)
        {
            var calculatedHealAmount = Mathf.RoundToInt(maxPlayerHealth * healPercentage);
            return calculatedHealAmount;
        }
    }
}