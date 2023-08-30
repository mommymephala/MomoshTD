using System;
using System.Collections.Generic;
using Containers;
using Controllers.Player_Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        public static UIManager Instance;
        public PlayerController playerController;

        [Header("UI Settings/References")]
        public GameObject upgradePanel;
        public Button upgradeButtonPrefab;
        public int buttonSpacing = 60;

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

        public void ShowUpgradePanel(List<UpgradeOption> upgradeOptions, Action<UpgradeOption> onUpgradeChosen)
        {
            upgradePanel.SetActive(true);

            // Clear any existing buttons
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

                // Position the button
                var yOffset = i * (buttonRectTransform.sizeDelta.y + buttonSpacing);
                buttonRectTransform.anchoredPosition = new Vector2(0f, -yOffset);

                // Set text and adjust font size
                var buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = option.description;

                // Calculate the preferred width of the text content
                var preferredWidth = LayoutUtility.GetPreferredWidth(buttonText.rectTransform);

                // Calculate the font size based on the preferred width
                var fontSize = 0.8f * buttonText.fontSize * (buttonRectTransform.sizeDelta.x / preferredWidth);
                buttonText.fontSize = Mathf.FloorToInt(fontSize); // Adjust the font size

                button.onClick.AddListener(() => onUpgradeChosen(option));
                button.onClick.AddListener(() => playerController.OnLevelUp());
            }
        }

        public void HideUpgradePanel()
        {
            upgradePanel.SetActive(false);
        }
    }
}
