using System.Collections.Generic;
using UnityEngine;

public class WeaponInitializer : MonoBehaviour
{
    private List<MeleeWeapon> meleeWeapons;
    private List<RangedWeapon> rangedWeapons;

    private IAttack attack;
    private IReload reload;

    private Transform trans;

    private void Awake()
    {
        trans = transform;

        attack = GetComponent<IAttack>();
        reload = GetComponent<IReload>();
    }

    private void Start() => TryInitializingAllWeapons();

    public bool TryInitializingAllWeapons()
    {
        GetAllWeapons();

        if (attack is null || reload is null) return false;

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
        trans.GetComponentsInChildren(meleeWeapons);
        trans.GetComponentsInChildren(rangedWeapons);
    }
}