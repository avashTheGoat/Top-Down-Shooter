using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Tooltip("Should be a child of the weapon object.")]
    [field: SerializeField] public Collider2D AttackAOE { get; protected set; }

    public List<GameObject> GetGameObjectsInAttackAOE()
    {
        List<RaycastHit2D> _hits = new();

        AttackAOE.Cast(Vector2.zero, new ContactFilter2D().NoFilter(), _hits);

        List<GameObject> _hitObjects = new();
        _hits.ForEach((_raycastHit) => { _hitObjects.Add(_raycastHit.collider.gameObject); });

        return _hitObjects;
    }

    public override void ResetWeapon()
    {
        
    }
}