using UnityEngine;
using System.Collections.Generic;

public class WeaponModStatsProvider : MonoBehaviour, IItemStatsProvider
{
    public List<string> ProvideStats(Item _item)
    {
        if (_item is not WeaponMod)
            throw new System.ArgumentException("Item must be a WeaponMod.");

        WeaponMod _mod = (WeaponMod)_item;

        List<string> _stats = new();
        if (_mod.ModSettings.DamageMulti != 1f)
            _stats.Add($"Damage Multi: {_mod.ModSettings.DamageMulti:0.0}");

        if (_mod.ModSettings.AttacksPerSecondMulti != 1f)
            _stats.Add($"Attack Rate Multi: {_mod.ModSettings.DamageMulti:0.0}");

        if (_mod.ModSettings.ReloadTimeMulti != 1f)
            _stats.Add($"Reload Time Multi: {_mod.ModSettings.ReloadTimeMulti:0.0}");

        if (_mod.ModSettings.MaxAmmoMulti != 1f)
            _stats.Add($"Max Ammo Multi: {_mod.ModSettings.MaxAmmoMulti:0.0}");

        if (_mod.ModSettings.ProjectileSpeedMulti != 1f)
            _stats.Add($"Projectile Speed Multi: {_mod.ModSettings.ProjectileSpeedMulti:0.0}");

        if (_mod.ModSettings.ProjectileRangeMulti != 1f)
            _stats.Add($"Projectile Range Multi: {_mod.ModSettings.ProjectileRangeMulti:0.0}");

        if (_mod.ModSettings.NumProjectilesShotIncrease != 0)
            _stats.Add($"Projectiles Shot Increase: {_mod.ModSettings.NumProjectilesShotIncrease}");

        if (_mod.ModSettings.AccuracyMulti != 1f)
            _stats.Add($"Accuracy Multi: {_mod.ModSettings.AccuracyMulti}");

        return _stats;
    }
}