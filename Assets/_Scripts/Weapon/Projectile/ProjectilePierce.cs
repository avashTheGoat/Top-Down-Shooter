using System;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class ProjectilePierce : MonoBehaviour
{
    public event Action<GameObject, GameObject, float> OnObjectCollision;

    [HideInInspector]
    public int PierceCounter = 0;

    [field: SerializeField] public int MaxPierces { get; private set; }

    private Projectile projectile;

    private void Awake() => projectile = GetComponent<Projectile>();

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject == gameObject) return;
        if (_col.isTrigger) return;

        OnObjectCollision?.Invoke(gameObject, _col.gameObject, projectile.Damage);
    }
}