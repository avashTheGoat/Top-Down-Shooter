using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(IAttack), typeof(IReload))]
public class WeaponInitializer : MonoBehaviour
{
    [Header("Transform Changes")]
    [SerializeField] private Vector2 weaponOffset;
    [SerializeField] private Vector2 weaponSizeMultiplier;
    [Space(15)]

    [SerializeField] private List<string> tagsToIgnore;

    private IAttack attack;
    private IReload reload;

    private Transform trans;

    private void Awake()
    {
        if (weaponSizeMultiplier.x == 0 || weaponSizeMultiplier.y == 0)
            throw new System.Exception($"{nameof(weaponSizeMultiplier)}.x or {nameof(weaponSizeMultiplier)}.y is 0.");

        trans = transform;
    }

    public void ApplyWeaponTransformChange(Weapon _weapon)
    {
        Transform _weaponTrans = _weapon.transform;

        _weaponTrans.localPosition += (Vector3) weaponOffset;
        _weaponTrans.localScale = new Vector2
        (
            _weaponTrans.localScale.x * weaponSizeMultiplier.x,
            _weaponTrans.localScale.y * weaponSizeMultiplier.y
        );
    }

    public void InitializeWeapon(Weapon _weapon)
    {
        if (attack == null)
            throw new System.ArgumentNullException("attack cannot be null.");

        _weapon.SetWielder(trans);

        if (_weapon is RangedWeapon)
        {
            if (reload == null)
                throw new System.ArgumentNullException("attack cannot be null.");

            RangedWeapon _rangedWeapon = _weapon as RangedWeapon;
            _rangedWeapon.SetWeaponLogic(attack, reload);
        }

        else if (_weapon is MeleeWeapon)
            _weapon.SetWeaponLogic(attack);

        else
            throw new System.Exception("weapon type not recognized. It is " + _weapon.GetType());
    }

    public void InitializeWeaponWielder(Weapon _weapon) => _weapon.SetWielder(trans);

    public void InitializeTagsToIgnore(Weapon _weapon) => _weapon.SetTagsToIgnore(tagsToIgnore);

    public void SetAttackAndReload(IAttack _newAttack, IReload _newReload)
    {
        attack = _newAttack;
        reload = _newReload;
    }
}