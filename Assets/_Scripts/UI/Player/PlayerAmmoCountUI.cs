using UnityEngine;
using TMPro;

public class PlayerAmmoCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private void Update()
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            PlayerWeaponsSwitcher _weaponsSwitcher = _player.GetComponent<PlayerWeaponsSwitcher>();
            if (!(_weaponsSwitcher.ActiveWeapon is RangedWeapon))
            {
                ammoText.text = "";
                return;
            }

            RangedWeapon _activeWeapon = _weaponsSwitcher.ActiveWeapon as RangedWeapon;
            ammoText.text = $"{_activeWeapon.Ammo}/{_activeWeapon.MaxAmmo}";
        }
    }
}