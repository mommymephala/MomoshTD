using System;
using System.Collections.Generic;
using UnityEngine;

namespace Containers
{
    [Serializable]
    public class PlayerData
    {
        public Dictionary<UpgradeType, int> attributeLevels = new Dictionary<UpgradeType, int>();

        // Property for the maximum level for permanent upgrades
        public int MaxPermanentLevel { get; private set; } = 5;

        // Property for the maximum level for in-game upgrades
        public int MaxInGameLevel { get; private set; } = 10;

        public PlayerData()
        {
            // Initialize the dictionary with default values for all upgrade types
            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                attributeLevels[upgradeType] = 0;
            }
        }

        public int MaxLevelForAttribute(UpgradeType upgradeType)
        {
            // Check if the attribute has been permanently upgraded
            var currentLevel = attributeLevels[upgradeType];
            return currentLevel <= MaxPermanentLevel ?
                // The player can upgrade up to the in-game maximum level
                Math.Min(MaxInGameLevel, MaxPermanentLevel + currentLevel) : MaxInGameLevel;
        }
    }
}