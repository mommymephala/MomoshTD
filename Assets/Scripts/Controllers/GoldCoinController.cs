using UnityEngine;

namespace Controllers
{
    public class GoldCoinController : MonoBehaviour
    {
        private readonly int _goldAmount = 1;
        
        public int GetGoldAmount()
        {
            return _goldAmount;
        }
    }
}