using System.Collections.Generic;
using UnityEngine;

public class BowProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private List<Component> weaponProviders;

    private List<ProjectilePierce> bowArrowPierces = new();
    private List<ProjectilePierce> subscribedBowArrowPierces = new();

    private void Awake() => UpdateBowArrowHits();

    private void Update()
    {
        UpdateBowArrowHits();
        subscribedBowArrowPierces.RemoveAll(projectile => projectile == null);

        foreach (ProjectilePierce _bowArrowPierce in bowArrowPierces)
        {
            if (subscribedBowArrowPierces.Contains(_bowArrowPierce)) continue;
            subscribedBowArrowPierces.Add(_bowArrowPierce);

            _bowArrowPierce.OnObjectCollision += Debug;
            _bowArrowPierce.OnObjectCollision += ArrowHitLogic;
        }
    }

    private void Debug(GameObject _, GameObject _hitObject, float __) => print($"Bow arrow hit something. Name is {_hitObject.name}");

    private void ArrowHitLogic(GameObject _arrow,  GameObject _hitObject, float _damageAmount)
    {
        if (_hitObject.TryGetComponent(out IDamageable _damageable))
        {
            _damageable.Damage(_damageAmount);

            ProjectilePierce _arrowPierce = _arrow.GetComponent<ProjectilePierce>();
            _arrowPierce.PierceCounter++;
            if (_arrowPierce.PierceCounter > _arrowPierce.MaxPierces) Destroy(_arrow);
        }

        else Destroy(_arrow);
    }

    private void UpdateBowArrowHits()
    {
        bowArrowPierces.Clear();
        weaponProviders.RemoveAll(_weaponProvider => _weaponProvider == null);

        foreach (IWeaponProvider _weaponProvider in weaponProviders)
        {
            foreach (BowWeapon _pistol in _weaponProvider.GetWeapons<BowWeapon>())
            {
                _pistol.GetShotProjectiles().ForEach((_projectile) => bowArrowPierces.Add(_projectile.GetComponent<ProjectilePierce>()));
            }
        }
    }
}