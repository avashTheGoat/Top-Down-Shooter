using System.Collections.Generic;
using UnityEngine;

public class PistolProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private PistolWeapon pistol;

    private List<ProjectileHit> pistolBulletHits = new();
    private List<ProjectileHit> subscribedPistolBulletHits = new();

    private void Awake()
    {
        pistol.OnWeaponAttack += UpdatePistolBulletHits;
    }

    private void Update()
    {
        subscribedPistolBulletHits.RemoveAll((projectile) => { return projectile == null; });

        List<ProjectileHit> _pistolBulletHitsToRemove = new();

        for (int i = 0; i < pistolBulletHits.Count; i++)
        {
            ProjectileHit _pistolBulletHit = pistolBulletHits[i];

            if (_pistolBulletHit == null)
            {
                _pistolBulletHitsToRemove.Add(_pistolBulletHit);
                continue;
            }

            if (subscribedPistolBulletHits.Contains(_pistolBulletHit)) continue;

            _pistolBulletHit.OnObjectCollision += Debug;
            _pistolBulletHit.OnObjectCollision += DamageIfDamageable;
            _pistolBulletHit.OnObjectCollision += DestroyBullet;

            subscribedPistolBulletHits.Add(_pistolBulletHit);
        }

        foreach (ProjectileHit _pistolBulletHitToRemove in _pistolBulletHitsToRemove)
        {
            pistolBulletHits.Remove(_pistolBulletHitToRemove);
        }
    }

    private void Debug(GameObject __, GameObject hitObject, float _) => print($"Pistol bullet hit something. Name is {hitObject.name}");

    private void DamageIfDamageable(GameObject _, GameObject hitObject, float damageAmount)
    {
        if (hitObject.TryGetComponent(out IDamageable _damageable))
        {
            _damageable.Damage(damageAmount);
        }
    }

    private void DestroyBullet(GameObject bullet, GameObject _, float __) => Destroy(bullet);

    private void UpdatePistolBulletHits() => pistolBulletHits.Add(pistol.ShotProjectiles[^1].GetComponent<ProjectileHit>());
}