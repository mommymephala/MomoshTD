using TMPro;
using UnityEngine;

namespace Controllers.Managers
{
    public class TimeControl : MonoBehaviour
    {
        public static TimeControl Instance { get; private set; }
        public TextMeshProUGUI buttonText;
        private readonly float[] _timeScales = { 1.0f, 2.0f, 3.0f };
        private int _timeScaleIndex;
        public float previousTimeScale;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateButtonText();
        }

        public void ToggleTime()
        {
            _timeScaleIndex = (_timeScaleIndex + 1) % _timeScales.Length;
            previousTimeScale = Time.timeScale;
            Time.timeScale = _timeScales[_timeScaleIndex];
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            buttonText.text = "Time x" + _timeScales[_timeScaleIndex];
        }
    }
}