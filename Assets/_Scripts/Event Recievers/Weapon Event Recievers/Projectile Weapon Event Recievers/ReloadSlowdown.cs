using UnityEngine;
using System.Collections.Generic;

public class ReloadSlowdown : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Component> rangedWeaponProviderComponents = new();
    [SerializeField] private PlayerWeaponsSwitcher weaponsSwitcher;
    [Space(15)]

    [Range(0f, 1f)]
    [SerializeField] private float newMovementSpeedPercent;

    private List<IProvider<RangedWeapon>> rangedWeaponProviders = new();

    private List<RangedWeapon> subscribedWeapons = new();
    private bool wasReloading = false;

    private void Start()
    {
        foreach (Component _component in rangedWeaponProviderComponents)
        {
            if (!(_component is IProvider<RangedWeapon>))
                throw new System.Exception("Given component must be a ranged weapon provider.");

            rangedWeaponProviders.Add((IProvider<RangedWeapon>)_component);
        }

        weaponsSwitcher.OnWeaponSwitch += (_prevWeapon, _newWeapon) =>
        {
            if (!wasReloading)
                return;

            if (PlayerProvider.TryGetPlayer(out Transform _player))
            {
                IMover _playerMover = _player.GetComponent<IMover>();
                _playerMover.SetMovementSpeed(_playerMover.GetOriginalMovementSpeed());
            }
        };
    }

    private void Update()
    {
        List<RangedWeapon> _rangedWeapons = new();
        foreach (IProvider<RangedWeapon> _provider in rangedWeaponProviders)
            _rangedWeapons.AddRange(_provider.Provide());

        subscribedWeapons.RemoveAll(_weapon => _weapon == null);

        foreach (RangedWeapon _weapon in _rangedWeapons)
        {
            if (_weapon == null)
                continue;

            if (subscribedWeapons.Contains(_weapon))
                continue;

            if (_weapon.Wielder.TryGetComponent(out IMover _moveable))
            {
                _weapon.OnReload += _ =>
                {
                    _moveable.SetMovementSpeed(_moveable.GetMovementSpeed() * newMovementSpeedPercent);
                    wasReloading = true;
                };

                _weapon.OnReloadComplete += _ =>
                {
                    _moveable.SetMovementSpeed(_moveable.GetOriginalMovementSpeed());
                    wasReloading = false;
                };
            }
        }
    }
}