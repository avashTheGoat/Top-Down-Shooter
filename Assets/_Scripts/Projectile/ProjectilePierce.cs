using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class ProjectilePierce : ProjectileHit
{
    [HideInInspector]
    public int PierceCounter = 0;

    [field: SerializeField] public int MaxPierces { get; private set; }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (TagsToIgnore.Contains(_col.gameObject.tag))
            return;

        if (_col.gameObject == gameObject)
            return;

        if (_col.isTrigger)
            return;

        InvokeOnObjectCollision(_col.gameObject);
    }
}