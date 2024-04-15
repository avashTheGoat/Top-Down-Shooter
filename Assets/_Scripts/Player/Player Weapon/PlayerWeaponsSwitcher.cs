using UnityEngine;
using System;

[RequireComponent(typeof(PlayerWeaponsManager))]
public class PlayerWeaponsSwitcher : MonoBehaviour
{
    public event Action<Weapon, Weapon> OnWeaponSwitch;

    public Weapon ActiveWeapon { get; private set; }
    
    [SerializeField] private KeyCode weaponSwitchKey;

    private PlayerWeaponsManager weaponsManager;
    private int curWeaponIndex = 0;

    private bool isBowWeaponFound = false;
    private bool isBowCharging = false;

    private void Awake() => weaponsManager = GetComponent<PlayerWeaponsManager>();

    private void Start()
    {
        foreach (Weapon _weapon in weaponsManager.GetWeapons())
            _weapon.gameObject.SetActive(false);

        ActiveWeapon = weaponsManager.PlayerWeapons[0];
        ActiveWeapon.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isBowWeaponFound)
        {
            foreach (RangedWeapon _rangedWeapon in weaponsManager.GetRangedWeapons())
            {
                if (_rangedWeapon is BowWeapon)
                {
                    isBowWeaponFound = true;

                    BowWeapon _bow = (BowWeapon)_rangedWeapon;
                    _bow.OnBowCharge += (_, __) => isBowCharging = true;
                    _bow.OnAttackWithoutAmmo += _ => isBowCharging = false;
                    _bow.OnAttack += _ => isBowCharging = false;
                }
            }
        }

        if (Input.GetKeyDown(weaponSwitchKey) && !isBowCharging)
        {
            int _newWeaponIndex = (curWeaponIndex + 1) % weaponsManager.PlayerWeapons.Count;
            SwitchWeapon(_newWeaponIndex);
            curWeaponIndex = _newWeaponIndex;
        }
    }

    private void SwitchWeapon(int _weaponIndex)
    {
        Weapon _prevWeapon = ActiveWeapon;

        ActiveWeapon.gameObject.SetActive(false);
        ActiveWeapon.ResetWeapon();

        ActiveWeapon = weaponsManager.PlayerWeapons[_weaponIndex];
        ActiveWeapon.gameObject.SetActive(true);

        OnWeaponSwitch?.Invoke(_prevWeapon, ActiveWeapon);
    }
}