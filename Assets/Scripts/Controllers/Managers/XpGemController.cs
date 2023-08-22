using UnityEngine;

namespace Controllers.Managers
{
    public class XpGemController : MonoBehaviour
    {
        [SerializeField] private float maxXpMultiplierTime; // Represents the duration in seconds over which you want the scaling to occur.
        [SerializeField] private int minXpAmount;
        [SerializeField] private int maxXpAmount;
        [SerializeField] private float maxXpMultiplier;

        private void Update()
        {
            CalculateXpMultiplier();
        }

        public float GetXpAmount()
        {
            var xpMultiplier = CalculateXpMultiplier();
            var xpAmount = Random.Range(minXpAmount, maxXpAmount + 1) * xpMultiplier;
            return xpAmount;
        }

        private float CalculateXpMultiplier()
        {
            var xpMultiplier = Mathf.Lerp(1.0f, maxXpMultiplier, Mathf.Clamp01(Time.time / maxXpMultiplierTime));
            return xpMultiplier;
        }
    }
}