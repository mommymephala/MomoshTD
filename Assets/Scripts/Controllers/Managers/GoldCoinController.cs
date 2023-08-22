using UnityEngine;

namespace Controllers.Managers
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