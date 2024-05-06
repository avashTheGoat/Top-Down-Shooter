using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerWeaponReloadReciever : MonoBehaviour
{
    [SerializeField] private Component rangedWeaponsProviderComponent;
    [SerializeField] private PlayerWeaponsSwitcher weaponsSwitcher;

    [SerializeField] private PlayerWeaponReloadUI reload;
    [SerializeField] private Image defaultCrosshair;

    private List<RangedWeapon> subscribedRangedWeapons = new();
    private IProvider<List<RangedWeapon>> rangedWeaponsProvider;

    private GameObject reloadParent;

    private void Start()
    {
        rangedWeaponsProvider = (IProvider<List<RangedWeapon>>)rangedWeaponsProviderComponent;
        weaponsSwitcher.OnWeaponSwitch += (_, __) => ResetCursor();

        reloadParent = reload.transform.parent.gameObject;
    }

    private void Update()
    {
        foreach (RangedWeapon _rangedWeapon in rangedWeaponsProvider.Provide())
        {
            if (subscribedRangedWeapons.Contains(_rangedWeapon))
                continue;

            _rangedWeapon.OnReload += _ =>
            {
                defaultCrosshair.gameObject.SetActive(false);

                reloadParent.gameObject.SetActive(true);
                reload.SetRangedWeapon(_rangedWeapon);
            };
            _rangedWeapon.OnReloadComplete += _ => ResetCursor();

            subscribedRangedWeapons.Add(_rangedWeapon);
        }
    }

    private void ResetCursor()
    {
        reloadParent.gameObject.SetActive(false);
        reload.SetFillAmount(0f);

        defaultCrosshair.gameObject.SetActive(true);
    }
}