using System;

namespace Containers
{
    [Serializable]
    public class UpgradeOption
    {
        public UpgradeType type;
        public string description;
        // TODO: Possibly more fields
    }

    public enum UpgradeType
    {
        WeaponDamage,
        ProjectileSpeed,
        WeaponCooldown,
        AoeEffect,
        TowerMaxHp,
        HealthRegenAmount,
        AddNewWeapon,
        // TODO: Add other possible upgrade types
    }
}