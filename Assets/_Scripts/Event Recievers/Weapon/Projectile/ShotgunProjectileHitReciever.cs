using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private ShotgunWeapon shotgun;

    private List<ProjectileHit> shotgunBulletHits = new();
    private List<ProjectileHit> subscribedShotgunBulletHits = new();

    private void Awake()
    {
        shotgun.OnWeaponAttack += UpdateShotgunBulletHits;
    }

    private void Update()
    {
        subscribedShotgunBulletHits.RemoveAll((projectile) => { return projectile == null; });

        for (int i = 0; i < shotgunBulletHits.Count; i++)
        {
            ProjectileHit _shotgunBulletHit = shotgunBulletHits[i];

            if (_shotgunBulletHit == null)
            {
                continue;
            }

            if (subscribedShotgunBulletHits.Contains(_shotgunBulletHit)) continue;

            _shotgunBulletHit.OnObjectCollision += Debug;
            _shotgunBulletHit.OnObjectCollision += DamageIfDamageable;
            _shotgunBulletHit.OnObjectCollision += DestroyBullet;

            subscribedShotgunBulletHits.Add(_shotgunBulletHit);
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
        shotgun.ShotProjectiles.RemoveAll((_bullet) => { return _bullet == null; });
        shotgunBulletHits.Clear();
        shotgun.ShotProjectiles.ForEach((_bulletHit) => { shotgunBulletHits.Add(_bulletHit.GetComponent<ProjectileHit>()); });
    }
}