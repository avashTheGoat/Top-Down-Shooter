using System.Collections.Generic;
using UnityEngine;

public class BowProjectileHitReciever : MonoBehaviour
{
    [SerializeField] private BowWeapon bow;

    private List<ProjectilePierce> bowArrowHits = new();
    private List<ProjectilePierce> subscribedBowArrowPierces = new();

    private void Awake() => bow.OnWeaponAttack += UpdateBowArrowHits;

    private void Update()
    {
        subscribedBowArrowPierces.RemoveAll((projectile) => { return projectile == null; });

        List<int> _bowArrowHitIndexesToRemove = new();

        for (int i = 0; i < bowArrowHits.Count; i++)
        {
            ProjectilePierce _bowArrowHit = bowArrowHits[i];

            if (_bowArrowHit == null)
            {
                _bowArrowHitIndexesToRemove.Add(i);
                continue;
            }

            if (subscribedBowArrowPierces.Contains(_bowArrowHit)) continue;

            _bowArrowHit.OnObjectCollision += DebugHitObjectName;
            _bowArrowHit.OnObjectCollision += DamageableLogic;

            subscribedBowArrowPierces.Add(_bowArrowHit);
        }

        foreach (int _pistolBulletHitIndexToRemove in _bowArrowHitIndexesToRemove)
        {
            bowArrowHits.RemoveAt(_pistolBulletHitIndexToRemove);
        }
    }

    private void DebugHitObjectName(GameObject _, GameObject _hitObject, float __) => print($"Bow arrow hit something. Name is {_hitObject.name}");

    private void DamageableLogic(GameObject _arrow,  GameObject _hitObject, float _damageAmount)
    {
        if (_hitObject.TryGetComponent(out IDamageable _damageable))
        {
            _damageable.Damage(_damageAmount);

            ProjectilePierce _arrowPierce = _arrow.GetComponent<ProjectilePierce>();
            _arrowPierce.PierceCounter++;
            if (_arrowPierce.PierceCounter > _arrowPierce.MaxPierces) Destroy(_arrow);
        }

        else
        {
            Destroy(_arrow);
        }
    }

    private void UpdateBowArrowHits() => bowArrowHits.Add(bow.ShotProjectiles[^1].GetComponent<ProjectilePierce>());
}