using System;

namespace Containers
{
    [Serializable]
    public class UpgradeOption
    {
        public UpgradeType type;
        public string description;
    }

    public enum UpgradeType
    {
        WeaponDamage,
        ProjectileSpeed,
        WeaponCooldown,
        AoeEffect,
        TowerMaxHp,
        HealthRegenAmount,
        AddNewWeapon
    }
}