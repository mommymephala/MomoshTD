using UnityEngine;

namespace Controllers
{
    public class GoldCoinController : MonoBehaviour
    {
        private const int GoldAmount = 1;

        public int GetGoldAmount()
        {
            return GoldAmount;
        }
    }
}