using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerWeaponReloadReciever : MonoBehaviour
{
    [SerializeField] private Component rangedWeaponProviderComponent;
    [SerializeField] private PlayerWeaponsSwitcher weaponsSwitcher;

    [SerializeField] private PlayerWeaponReloadUI reload;
    [SerializeField] private Image defaultCrosshair;

    private List<RangedWeapon> subscribedRangedWeapons = new();
    private IProvider<RangedWeapon> rangedWeaponProvider;

    private GameObject reloadParent;

    private void Start()
    {
        rangedWeaponProvider = (IProvider<RangedWeapon>)rangedWeaponProviderComponent;
        weaponsSwitcher.OnWeaponSwitch += ResetCursor;

        reloadParent = reload.transform.parent.gameObject;
    }

    private void Update()
    {
        foreach (RangedWeapon _rangedWeapon in rangedWeaponProvider.Provide())
        {
            if (subscribedRangedWeapons.Contains(_rangedWeapon))
                continue;

            _rangedWeapon.OnReload += () =>
            {
                defaultCrosshair.gameObject.SetActive(false);

                reloadParent.gameObject.SetActive(true);
                reload.SetRangedWeapon(_rangedWeapon);
            };
            _rangedWeapon.OnReloadComplete += ResetCursor;

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