using System;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class ProjectileHit : MonoBehaviour
{
    public event Action<GameObject, GameObject, float> OnObjectCollision;

    private Projectile projectile;

    private void Awake() => projectile = GetComponent<Projectile>();

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.gameObject == gameObject) return;

        OnObjectCollision?.Invoke(gameObject, _col.gameObject, projectile.Damage);
    }
}