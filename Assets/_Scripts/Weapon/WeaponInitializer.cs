using System;
using UnityEngine;

public class WeaponInitializer : MonoBehaviour
{
    [SerializeField] private bool shouldApplyWeaponTransformChanges = false;
    [SerializeField] private Vector2 weaponOffset;
    [SerializeField] private Vector2 weaponSizeMultiplier;

    private Component[] meleeWeapons;
    private Component[] rangedWeapons;

    private IAttack attack;
    private IReload reload;

    private Transform trans;

    private void Awake()
    {
        trans = transform;

        attack = GetComponent<IAttack>();
        reload = GetComponent<IReload>();
    }

    private void Start()
    {
        TryInitializingAllWeaponAttacksAndReloads();
        if (shouldApplyWeaponTransformChanges) ApplyWeaponTransformChange();
    }

    public void ApplyWeaponTransformChange()
    {
        foreach (Component _weapon in meleeWeapons)
        {
            Transform _weaponTrans = _weapon.transform;

            _weaponTrans.localPosition += (Vector3) weaponOffset;
            _weaponTrans.localScale = weaponSizeMultiplier;
        }

        foreach (Component _weapon in rangedWeapons)
        {
            Transform _weaponTrans = _weapon.transform;

            _weaponTrans.localPosition += (Vector3) weaponOffset;
            _weaponTrans.localScale = new Vector2(_weaponTrans.localScale.x * weaponSizeMultiplier.x,
                                                  _weaponTrans.localScale.y * weaponSizeMultiplier.y);
        }
    }

    public bool TryInitializingAllWeaponAttacksAndReloads()
    {
        if (attack is null || reload is null) return false;

        GetAllWeapons();

        foreach (MeleeWeapon _meleeWeapon in meleeWeapons)
            _meleeWeapon.Init(transform, attack);

        foreach (RangedWeapon _rangedWeapon in rangedWeapons)
            _rangedWeapon.Init(transform, attack, reload);

        return true;
    }

    public void SetAttackAndReload(IAttack _newAttack, IReload _newReload)
    {
        attack = _newAttack;
        reload = _newReload;
    }

    private void GetAllWeapons()
    {
        meleeWeapons = trans.GetComponentsInChildren<MeleeWeapon>(true);
        rangedWeapons = trans.GetComponentsInChildren<RangedWeapon>(true);
    }
}