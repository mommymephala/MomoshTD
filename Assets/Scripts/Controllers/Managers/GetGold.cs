using TMPro;
using UnityEngine;

namespace Controllers.Managers
{
   public class GetGold : MonoBehaviour
   {
      private TextMeshProUGUI _goldText;
      public int totalGold;
   
      private void Start()
      {
         _goldText = GameObject.FindWithTag("GoldText").GetComponent<TextMeshProUGUI>();
      }

      private void Update()
      {
         totalGold = PlayerPrefs.GetInt("TotalGold");
         _goldText.text = totalGold.ToString();
      }
   }
}
