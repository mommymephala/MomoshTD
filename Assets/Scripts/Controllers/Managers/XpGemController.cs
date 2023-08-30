using UnityEngine;

namespace Controllers.Managers
{
    public class XpGemController : MonoBehaviour
    {
        [SerializeField] private int minXpAmount;
        [SerializeField] private int maxXpAmount;

        public int GetXpAmount()
        {
            var xpAmount = Random.Range(minXpAmount, maxXpAmount + 1);
            return xpAmount;
        }
    }
}