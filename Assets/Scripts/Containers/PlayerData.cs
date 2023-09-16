using System;
using System.Collections.Generic;

namespace Containers
{
    [Serializable]
    public class PlayerData
    {
        public Dictionary<UpgradeType, int> attributeLevels = new Dictionary<UpgradeType, int>();

        public int MaxPermanentLevel { get; private set; } = 5;

        public int MaxInGameLevel { get; private set; } = 10;

        public PlayerData()
        {
            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
            {
                attributeLevels[upgradeType] = 0;
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