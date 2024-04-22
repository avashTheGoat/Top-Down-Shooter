using System.Collections.Generic;
using UnityEngine;

public class PistolProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private List<Component> weaponProviders;

    private List<ProjectileHit> pistolBulletHits = new();
    private List<ProjectileHit> subscribedPistolBulletHits = new();

    private void Awake() => UpdatePistolBulletHits();

    private void Update()
    {
        UpdatePistolBulletHits();
        subscribedPistolBulletHits.RemoveAll(projectile => projectile == null);

        foreach (ProjectileHit _pistolBulletHit in pistolBulletHits)
        {
            if (subscribedPistolBulletHits.ContainsReference(_pistolBulletHit))
                continue;
            subscribedPistolBulletHits.Add(_pistolBulletHit);

            _pistolBulletHit.OnObjectCollision += DamageIfDamageable;
            _pistolBulletHit.OnObjectCollision += DestroyBullet;

        }
    }

    private void DamageIfDamageable(GameObject _, GameObject hitObject, float damageAmount)
    {
        if (hitObject.TryGetComponent(out IDamageable _damageable))
        {
            _damageable.Damage(damageAmount);
        }
    }

    private void DestroyBullet(GameObject bullet, GameObject _, float __) => Destroy(bullet);

    private void UpdatePistolBulletHits()
    {
        pistolBulletHits.Clear();
        weaponProviders.RemoveAll(_weaponProvider => _weaponProvider == null);

        foreach (IProvider<RangedWeapon> _weaponProvider in weaponProviders)
        {
            foreach (RangedWeapon _rangedWeapon in _weaponProvider.Provide())
            {
                if (_rangedWeapon is PistolWeapon)
                    _rangedWeapon.GetShotProjectiles().ForEach((_projectile) => pistolBulletHits.Add(_projectile.GetComponent<ProjectileHit>()));
            }
        }
    }
}