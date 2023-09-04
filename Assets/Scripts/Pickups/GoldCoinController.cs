using UnityEngine;

namespace Pickups
{
    public class GoldCoinController : MonoBehaviour
    {
        private const int GoldAmount = 1;

        public static int GetGoldAmount()
        {
            return GoldAmount;
        }
    }
}