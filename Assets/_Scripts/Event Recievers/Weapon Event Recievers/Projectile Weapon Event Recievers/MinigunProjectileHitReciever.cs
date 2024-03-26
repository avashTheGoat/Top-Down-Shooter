using System.Collections.Generic;
using UnityEngine;

public class MinigunProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private List<Component> weaponProviders;

    private List<ProjectileHit> minigunBulletHits = new();
    private List<ProjectileHit> subscribedMinigunBulletHits = new();

    private void Awake() => UpdateMinigunBulletHits();

    private void Update()
    {
        UpdateMinigunBulletHits();
        subscribedMinigunBulletHits.RemoveAll(projectile => projectile == null);

        foreach (ProjectileHit _minigunBulletHit in minigunBulletHits)
        {
            if (subscribedMinigunBulletHits.Contains(_minigunBulletHit)) continue;
            subscribedMinigunBulletHits.Add(_minigunBulletHit);

            _minigunBulletHit.OnObjectCollision += Debug;
            _minigunBulletHit.OnObjectCollision += DamageIfDamageable;
            _minigunBulletHit.OnObjectCollision += DestroyBullet;

        }
    }

    private void Debug(GameObject _, GameObject hitObject, float __) => print($"Minigun bullet hit something. Name is {hitObject.name}");

    private void DamageIfDamageable(GameObject _, GameObject hitObject, float damageAmount)
    {
        if (hitObject.TryGetComponent(out IDamageable _damageable))
        {
            _damageable.Damage(damageAmount);
        }
    }

    private void DestroyBullet(GameObject bullet, GameObject _, float __) => Destroy(bullet);

    private void UpdateMinigunBulletHits()
    {
        minigunBulletHits.Clear();
        weaponProviders.RemoveAll(_weaponProvider => _weaponProvider == null);

        foreach (IProvider<RangedWeapon> _weaponProvider in weaponProviders)
        {
            foreach (RangedWeapon _rangedWeapon in _weaponProvider.Provide())
            {
                if (_rangedWeapon is MinigunWeapon)
                    _rangedWeapon.GetShotProjectiles().ForEach(_projectile => minigunBulletHits.Add(_projectile.GetComponent<ProjectileHit>()));
            }
        }
    }
}