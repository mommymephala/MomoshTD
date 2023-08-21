using TMPro;
using UnityEngine;

namespace Controllers
{
    public class TimeControl : MonoBehaviour
    {
        public TextMeshProUGUI buttonText;
        private int _timeScaleIndex;
        private readonly float[] _timeScales = { 1.0f, 2.0f, 3.0f };

        private void Start()
        {
            UpdateButtonText();
        }

        public void ToggleTime()
        {
            _timeScaleIndex = (_timeScaleIndex + 1) % _timeScales.Length;
            Time.timeScale = _timeScales[_timeScaleIndex];
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            buttonText.text = "Time x" + _timeScales[_timeScaleIndex];
        }
    }
}