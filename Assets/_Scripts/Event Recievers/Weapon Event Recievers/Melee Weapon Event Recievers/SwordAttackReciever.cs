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
            if (subscribedSwords.ContainsReference(_sword))
                continue;
            subscribedSwords.Add(_sword);

            _sword.OnAttack += DamageObjectsInCollider;
        }
    }

    private void DamageObjectsInCollider(GameObject _sword)
    {
        SwordWeapon _swordWeapon = _sword.GetComponent<SwordWeapon>();
        foreach (GameObject _hitObject in _swordWeapon.GetGameObjectsInAttackAOE())
        {
            if (_hitObject.Equals(_swordWeapon.Wielder)) continue;

            if (_hitObject.TryGetComponent(out IDamageable _damageable))
                _damageable.Damage(_swordWeapon.Damage);
        }
    }

    private void UpdateSwords()
    {
        swords.Clear();
        weaponProviders.RemoveAll(_weaponProvider => _weaponProvider == null);

        foreach (IProvider<MeleeWeapon> _weaponProvider in weaponProviders)
        {
            foreach (MeleeWeapon _meleeWeapon in _weaponProvider.Provide())
            {
                if (_meleeWeapon is SwordWeapon)
                    swords.Add(_meleeWeapon as SwordWeapon);
            }
        }
    }
}