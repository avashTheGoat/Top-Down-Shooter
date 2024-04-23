using System.Collections.Generic;
using UnityEngine;

public class HammerAttackReciever : MonoBehaviour
{
    [SerializeField] private List<Component> weaponProviders;
    [SerializeField] private AnimationCurve percentOfOriginalDamageWhenXAwayFromCenter;

    private List<HammerWeapon> hammers = new();
    private List<HammerWeapon> subscribedHammers = new();

    private void Awake()
    {
        percentOfOriginalDamageWhenXAwayFromCenter.postWrapMode = WrapMode.Clamp;
        percentOfOriginalDamageWhenXAwayFromCenter.preWrapMode = WrapMode.Clamp;
    }

    private void Start() => UpdateHammers();

    private void Update()
    {
        UpdateHammers();
        subscribedHammers.RemoveAll(projectile => projectile == null);

        foreach (HammerWeapon _hammer in hammers)
        {
            if (subscribedHammers.ContainsReference(_hammer))
                continue;
            subscribedHammers.Add(_hammer);

            _hammer.OnAttack += DamageObjectsInCollider;
        }
    }

    private void DamageObjectsInCollider(GameObject _hammer)
    {
        HammerWeapon _hammerWeapon = _hammer.GetComponent<HammerWeapon>();

        foreach (GameObject _hitObject in _hammerWeapon.GetGameObjectsInAttackAOE())
        {
            if (_hitObject.Equals(_hammerWeapon.Wielder)) continue;

            if (_hitObject.TryGetComponent(out IDamageable _damageable))
            {
                float _distanceFromAttack = Vector2.Distance(_hammerWeapon.GetAttackAOECenter(), _hitObject.transform.position);
                float _damage = _hammerWeapon.Damage * percentOfOriginalDamageWhenXAwayFromCenter.Evaluate(_distanceFromAttack);
                _damageable.Damage(_damage);

                // print($"HAMMER (Damage, DistanceFromCenter): ({_damage}, {_distanceFromAttack})");
            }
        }
    }

    private void UpdateHammers()
    {
        hammers.Clear();
        weaponProviders.RemoveAll(_weaponProvider => _weaponProvider == null);

        foreach (IProvider<List<MeleeWeapon>>_weaponProvider in weaponProviders)
        {
            foreach (MeleeWeapon _meleeWeapon in _weaponProvider.Provide())
            {
                if (_meleeWeapon is HammerWeapon)
                    hammers.Add(_meleeWeapon as HammerWeapon);
            }
        }
    }
}