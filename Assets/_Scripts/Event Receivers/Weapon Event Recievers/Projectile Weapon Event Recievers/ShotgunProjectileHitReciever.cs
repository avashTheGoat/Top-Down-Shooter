using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private List<Component> weaponProviders;

    private List<ProjectileHit> shotgunBulletHits = new();
    private List<ProjectileHit> subscribedShotgunBulletHits = new();

    private void Start() => UpdateShotgunBulletHits();

    private void Update()
    {
        UpdateShotgunBulletHits();
        subscribedShotgunBulletHits.RemoveAll(projectile => projectile == null);

        foreach (ProjectileHit _shotgunBulletHit in shotgunBulletHits)
        {
            if (subscribedShotgunBulletHits.ContainsReference(_shotgunBulletHit))
                continue;
            subscribedShotgunBulletHits.Add(_shotgunBulletHit);

            _shotgunBulletHit.OnObjectCollision += DamageIfDamageable;
            _shotgunBulletHit.OnObjectCollision += DestroyBullet;
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

    private void UpdateShotgunBulletHits()
    {
        shotgunBulletHits.Clear();
        weaponProviders.RemoveAll(_weaponProvider => _weaponProvider == null);

        foreach (IProvider<List<RangedWeapon>> _weaponProvider in weaponProviders)
        {
            foreach (RangedWeapon _rangedWeapon in _weaponProvider.Provide())
            {
                if (_rangedWeapon is ShotgunWeapon)
                    _rangedWeapon.GetShotProjectiles().ForEach((_projectile) => shotgunBulletHits.Add(_projectile.GetComponent<ProjectileHit>()));
            }
        }
    }
}