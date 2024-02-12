using System.Collections.Generic;
using UnityEngine;

public class SwordAttackReciever : MonoBehaviour
{
    [SerializeField] private List<Component> weaponProviders;

    private List<SwordWeapon> swords = new();
    private List<SwordWeapon> subscribedSwords = new();

    private void Start() => UpdateSwords();

    private void Update()
    {
        UpdateSwords();
        subscribedSwords.RemoveAll(projectile => projectile == null);

        foreach (SwordWeapon _sword in swords)
        {
            if (subscribedSwords.Contains(_sword)) continue;
            subscribedSwords.Add(_sword);

            _sword.OnWeaponAttack += DamageObjectsInCollider;
        }
    }

    private void DamageObjectsInCollider(GameObject _sword)
    {
        SwordWeapon _swordWeapon = _sword.GetComponent<SwordWeapon>();
        foreach (GameObject _hitObject in _swordWeapon.GetGameObjectsInAttackAOE())
        {
            if (_hitObject.Equals(_swordWeapon.Wielder)) continue;

            if (_hitObject.TryGetComponent(out IDamageable _damageable))
                _damageable.Damage(_swordWeapon.WeaponDamage);
        }
    }

    private void UpdateSwords()
    {
        swords.Clear();
        weaponProviders.RemoveAll(_weaponProvider => _weaponProvider == null);

        foreach (IWeaponProvider _weaponProvider in weaponProviders)
        {
            foreach (SwordWeapon _sword in _weaponProvider.GetWeapons<SwordWeapon>())
                swords.Add(_sword);
        }
    }
}