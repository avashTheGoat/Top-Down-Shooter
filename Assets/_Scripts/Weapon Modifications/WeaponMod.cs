using UnityEngine;
using System.Collections.Generic;

public class WeaponMod : Item
{
	[SerializeField] private WeaponModSettings modSettings;

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

	private void ApplyModSettings(Weapon _target)
	{
		_target.AttacksPerSecond *= modSettings.AttacksPerSecondMulti;
		_target.Damage *= modSettings.DamageMulti;

		if (_target is RangedWeapon _rangedWeapon)
		{
			_rangedWeapon.MinAngleChange /= modSettings.AccuracyMulti;
			_rangedWeapon.MaxAngleChange /= modSettings.AccuracyMulti;

			_rangedWeapon.ReloadTime *= modSettings.ReloadTimeMulti;
			_rangedWeapon.Range *= modSettings.ProjectileRangeMulti;
			_rangedWeapon.ProjectileSpeed *= modSettings.ProjectileSpeedMulti;
			_rangedWeapon.NumBullets += modSettings.NumBulletsIncrease;
			_rangedWeapon.MaxAmmo = Mathf.RoundToInt(_rangedWeapon.MaxAmmo * modSettings.MaxBulletsMulti);
		}
	}

	private bool IsWeaponCorrectType(Weapon _weapon)
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
}
