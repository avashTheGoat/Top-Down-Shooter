using System.Collections.Generic;
using UnityEngine;

public class MinigunProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private MinigunWeapon minigun;

    private List<ProjectileHit> minigunBulletHits = new();
    private List<ProjectileHit> subscribedMinigunBulletHits = new();

    private void Awake()
    {
        minigun.OnWeaponAttack += UpdatePistolBulletHits;
    }

    private void Update()
    {
        subscribedMinigunBulletHits.RemoveAll((projectile) => { return projectile == null; });

        List<ProjectileHit> _minigunBulletHitsToRemove = new();

        for (int i = 0; i < minigunBulletHits.Count; i++)
        {
            ProjectileHit _minigunBulletHit = minigunBulletHits[i];

            if (_minigunBulletHit == null)
            {
                _minigunBulletHitsToRemove.Add(_minigunBulletHit);
                continue;
            }

            if (subscribedMinigunBulletHits.Contains(_minigunBulletHit)) continue;

            _minigunBulletHit.OnObjectCollision += Debug;
            _minigunBulletHit.OnObjectCollision += DamageIfDamageable;
            _minigunBulletHit.OnObjectCollision += DestroyBullet;

            subscribedMinigunBulletHits.Add(_minigunBulletHit);
        }

        foreach (ProjectileHit _minigunBulletHitToRemove in _minigunBulletHitsToRemove)
        {
            minigunBulletHits.Remove(_minigunBulletHitToRemove);
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

    private void UpdatePistolBulletHits() => minigunBulletHits.Add(minigun.ShotProjectiles[^1].GetComponent<ProjectileHit>());
}