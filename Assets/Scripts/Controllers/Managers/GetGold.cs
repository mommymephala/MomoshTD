using TMPro;
using UnityEngine;

namespace Controllers.Managers
{
   public class GetGold : MonoBehaviour
   {
      private TextMeshProUGUI _goldText;
      private int _totalGold;
   
      private void Start()
      {
         _goldText = GameObject.FindWithTag("GoldText").GetComponent<TextMeshProUGUI>();
      }

      private void Update()
      {
         _totalGold = PlayerPrefs.GetInt("TotalGold");
         _goldText.text = _totalGold.ToString();
      }
   }
}
