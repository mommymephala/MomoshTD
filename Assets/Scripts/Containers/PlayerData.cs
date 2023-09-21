using System;
using System.Collections.Generic;
using UnityEngine;

namespace Containers
{
    [Serializable]
    public class PlayerData
    {
        public Dictionary<UpgradeType, int> attributeLevels;

        public int MaxPermanentLevel { get; private set; } = 5;

        public int MaxInGameLevel { get; private set; } = 10;

        public void Initialize()
        {
            attributeLevels = new Dictionary<UpgradeType, int>();
            
            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                var savedLevel = PlayerPrefs.GetInt(upgradeType.ToString(), 0);
                attributeLevels[upgradeType] = savedLevel;
            }
        }

        public int MaxLevelForAttribute(UpgradeType upgradeType)
        {
            var currentLevel = attributeLevels[upgradeType];
            return currentLevel <= MaxPermanentLevel ?
                Math.Min(MaxInGameLevel, MaxPermanentLevel + currentLevel) : MaxInGameLevel;
        }
    }
}