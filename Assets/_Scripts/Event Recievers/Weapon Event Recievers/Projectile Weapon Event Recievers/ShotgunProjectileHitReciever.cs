using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private List<Component> weaponProviders;

    private List<ProjectileHit> shotgunBulletHits = new();
    private List<ProjectileHit> subscribedShotgunBulletHits = new();

    private void Awake() => UpdateShotgunBulletHits();

    private void Update()
    {
        UpdateShotgunBulletHits();
        subscribedShotgunBulletHits.RemoveAll(projectile => projectile == null);

        foreach (ProjectileHit _shotgunBulletHit in shotgunBulletHits)
        {
            if (subscribedShotgunBulletHits.Contains(_shotgunBulletHit)) continue;
            subscribedShotgunBulletHits.Add(_shotgunBulletHit);

            _shotgunBulletHit.OnObjectCollision += Debug;
            _shotgunBulletHit.OnObjectCollision += DamageIfDamageable;
            _shotgunBulletHit.OnObjectCollision += DestroyBullet;
        }
    }

    private void Debug(GameObject __, GameObject hitObject, float _) => print($"Shotgun bullet hit something. Name is {hitObject.name}");

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

        foreach (IWeaponProvider _weaponProvider in weaponProviders)
        {
            foreach (ShotgunWeapon _pistol in _weaponProvider.GetWeapons<ShotgunWeapon>())
            {
                _pistol.GetShotProjectiles().ForEach((_projectile) => shotgunBulletHits.Add(_projectile.GetComponent<ProjectileHit>()));
            }
        }
    }
}