using UnityEngine;

namespace Controllers
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

        public int GetXpAmount()
        {
            var xpMultiplier = CalculateXpMultiplier();
            var xpAmount = Mathf.RoundToInt(UnityEngine.Random.Range(minXpAmount, maxXpAmount + 1) * xpMultiplier);
            return xpAmount;   
        }

        private int CalculateXpMultiplier()
        {
            var xpMultiplier = Mathf.Lerp(1.0f, maxXpMultiplier, Mathf.Clamp01(Time.time / maxXpMultiplierTime));
            return (int)xpMultiplier;
        }
    }
}