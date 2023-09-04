using System;
using System.Collections.Generic;
using Containers;
using Controllers.Player_Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Managers
{
    public class InGameUpgradeUI : MonoBehaviour
    {
        [Header("References")]
        public static InGameUpgradeUI Instance;
        private PlayerController _playerController;

        [Header("UI Settings/References")]
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private Button upgradeButtonPrefab;
        [SerializeField] private int buttonSpacing = 60;

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
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void ShowUpgradePanel(List<UpgradeOption> upgradeOptions, Action<UpgradeOption> onUpgradeChosen)
        {
            if (_playerController == null)
            {
                return;
            }

            upgradePanel.SetActive(true);
            
            foreach (Transform child in upgradePanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Spacing between buttons
            for (var i = 0; i < upgradeOptions.Count; i++)
            {
                UpgradeOption option = upgradeOptions[i];
                Button button = Instantiate(upgradeButtonPrefab, upgradePanel.transform);
                var buttonRectTransform = button.GetComponent<RectTransform>();

                var yOffset = i * (buttonRectTransform.sizeDelta.y + buttonSpacing);
                buttonRectTransform.anchoredPosition = new Vector2(0f, -yOffset);

                var buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = option.description;

                var preferredWidth = LayoutUtility.GetPreferredWidth(buttonText.rectTransform);

                var fontSize = 0.8f * buttonText.fontSize * (buttonRectTransform.sizeDelta.x / preferredWidth);
                buttonText.fontSize = Mathf.FloorToInt(fontSize);

                button.onClick.AddListener(() => onUpgradeChosen(option));
                button.onClick.AddListener(() => _playerController.OnLevelUp());
            }
        }

        public void HideUpgradePanel()
        {
            upgradePanel.SetActive(false);
        }
    }
}
