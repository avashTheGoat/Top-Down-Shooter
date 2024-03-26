using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Projectile))]
public class ProjectileHit : MonoBehaviour
{
    public event Action<GameObject, GameObject, float> OnObjectCollision;

    public List<string> TagsToIgnore { get; private set; } = new();

    protected Projectile projectile;

    protected virtual void Awake() => projectile = GetComponent<Projectile>();

    protected virtual void OnCollisionEnter2D(Collision2D _col)
    {
        if (TagsToIgnore.Contains(_col.gameObject.tag))
            return;

        if (_col.gameObject == gameObject)
            return;

        OnObjectCollision?.Invoke(gameObject, _col.gameObject, projectile.Damage);
    }

    protected void InvokeOnObjectCollision(GameObject _colObject) => OnObjectCollision?.Invoke(gameObject, _colObject, projectile.Damage);
}