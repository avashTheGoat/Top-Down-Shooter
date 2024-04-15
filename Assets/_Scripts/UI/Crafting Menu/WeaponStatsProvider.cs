using UnityEngine;
using System.Collections.Generic;

public class WeaponStatsProvider : MonoBehaviour, IItemStatsProvider
{
    public List<string> ProvideStats(Item _item)
    {
        if (_item is not Weapon)
            throw new System.ArgumentException("_item must be a Weapon.");

        Weapon _weapon = (Weapon)_item;

        List<string> _stats = new();
        _stats.Add($"Damage: {_weapon.Damage:0.0}");
        _stats.Add($"Attack Rate: {_weapon.AttacksPerSecond:0.0}");

        if (_weapon is RangedWeapon _rangedWeapon)
        {
            _stats.Add($"Reload Speed: {_rangedWeapon.ReloadTime:0.0}");
            _stats.Add($"Ammo: {_rangedWeapon.MaxAmmo}");
            _stats.Add($"Projectile Speed: {_rangedWeapon.ProjectileSpeed:0.0}");
            _stats.Add($"Projectile Range: {_rangedWeapon.Range:0.0}");
            _stats.Add($"Num Shot Projectiles: {_rangedWeapon.NumProjectiles}");

            float _accuracy = 1 / Mathf.Max(1f, _rangedWeapon.MaxAngleChange + _rangedWeapon.MinAngleChange);
            _stats.Add($"Accuracy: {_accuracy:0.0}");
        }

        else if (_weapon is MeleeWeapon _meleeWeapon)
            _stats.Add($"AOE Size: {_meleeWeapon.AttackAOE.bounds.extents.magnitude:0.0}");

        else throw new System.Exception("Unrecognized weapon type.");

        return _stats;
    }
}