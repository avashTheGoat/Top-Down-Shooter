using UnityEngine;
using System.Collections.Generic;

public class WeaponMod : Item
{
	[field: SerializeField] public WeaponModSettings ModSettings { get; private set; }

	[Header("Type Restrictions")]
	[Tooltip("If false, this modification is only for ranged weapons, but if true, this modification is only for melee weapons.")]
	[SerializeField] private bool isForMeleeWeapon;

	[Header("Either Or (do not use both disallowed and allowed lists)")]
	[Tooltip("Attach any Weapon components that are disallowed and reference them in the list. If no disallowed types are provided, all types specified by the isForMeleeWeapon variable are allowed.")]
	[SerializeField] private List<Weapon> disallowedTypes;
	[Tooltip("Attach any Weapon components that are allowed and reference them in the list. If no allowed types are provided, all types specified by the isForMeleeWeapon variable are allowed.")]
	[SerializeField] private List<Weapon> allowedTypes;

	public bool ApplyMod(Weapon _target)
	{
		if (!IsWeaponCorrectType(_target))
			return false;

		ApplyModSettings(_target);
		return true;
	}

	public bool UnapplyMod(Weapon _target)
    {
		if (!IsWeaponCorrectType(_target))
			return false;

		UnapplyModSettings(_target);
		return true;
	}
	
	public bool IsWeaponCorrectType(Weapon _weapon)
	{
		if (_weapon is MeleeWeapon && !isForMeleeWeapon)
			return false;

		if (_weapon is RangedWeapon && isForMeleeWeapon)
			return false;

		if (disallowedTypes.Count == 0)
        {
			if (allowedTypes.Count == 0)
				return true;

			foreach (Weapon _allowedType in allowedTypes)
			{
				if (_weapon.GetType().Equals(_allowedType.GetType()))
					return true;
			}

			return false;
		}

		foreach (Weapon _disallowedType in disallowedTypes)
		{
            if (_weapon.GetType().Equals(_disallowedType.GetType()))
				return false;
		}

		return true;
	}

	private void ApplyModSettings(Weapon _target)
	{
		_target.AttacksPerSecond *= ModSettings.AttacksPerSecondMulti;
		_target.Damage *= ModSettings.DamageMulti;

		if (_target is RangedWeapon _rangedWeapon)
		{
			_rangedWeapon.MinAngleChange /= ModSettings.AccuracyMulti;
			_rangedWeapon.MaxAngleChange /= ModSettings.AccuracyMulti;

			_rangedWeapon.ReloadTime *= ModSettings.ReloadTimeMulti;
			_rangedWeapon.Range *= ModSettings.ProjectileRangeMulti;
			_rangedWeapon.ProjectileSpeed *= ModSettings.ProjectileSpeedMulti;
			_rangedWeapon.NumProjectiles += ModSettings.NumProjectilesShotIncrease;
			_rangedWeapon.MaxAmmo = Mathf.RoundToInt(_rangedWeapon.MaxAmmo * ModSettings.MaxAmmoMulti);
		}
	}

	private void UnapplyModSettings(Weapon _target)
    {
		_target.AttacksPerSecond /= ModSettings.AttacksPerSecondMulti;
		_target.Damage /= ModSettings.DamageMulti;

		if (_target is RangedWeapon _rangedWeapon)
		{
			_rangedWeapon.MinAngleChange *= ModSettings.AccuracyMulti;
			_rangedWeapon.MaxAngleChange *= ModSettings.AccuracyMulti;

			_rangedWeapon.ReloadTime /= ModSettings.ReloadTimeMulti;
			_rangedWeapon.Range /= ModSettings.ProjectileRangeMulti;
			_rangedWeapon.ProjectileSpeed /= ModSettings.ProjectileSpeedMulti;
			_rangedWeapon.NumProjectiles -= ModSettings.NumProjectilesShotIncrease;
			_rangedWeapon.MaxAmmo = Mathf.RoundToInt(_rangedWeapon.MaxAmmo / ModSettings.MaxAmmoMulti);
		}
	}

}