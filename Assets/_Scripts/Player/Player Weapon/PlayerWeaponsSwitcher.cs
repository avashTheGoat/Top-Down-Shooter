using UnityEngine;

[RequireComponent(typeof(PlayerWeaponsManager))]
public class PlayerWeaponsSwitcher : MonoBehaviour
{
    public Weapon ActiveWeapon { get; private set; }
    
    [SerializeField] private KeyCode weaponSwitchKey;

    private PlayerWeaponsManager weaponsManager;
    private int curWeaponIndex = 0;

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
        if (Input.GetKeyDown(weaponSwitchKey))
        {
            int _newWeaponIndex = (curWeaponIndex + 1) % weaponsManager.PlayerWeapons.Count;
            SwitchWeapon(_newWeaponIndex);
            curWeaponIndex = _newWeaponIndex;
        }
    }

    private void SwitchWeapon(int _weaponIndex)
    {
        ActiveWeapon.gameObject.SetActive(false);
        ActiveWeapon = weaponsManager.PlayerWeapons[_weaponIndex];
        ActiveWeapon.gameObject.SetActive(true);
    }
}