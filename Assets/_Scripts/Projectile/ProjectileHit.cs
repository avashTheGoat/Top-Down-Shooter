using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(ProjectileInfo), typeof(Collider2D))]
public class ProjectileHit : MonoBehaviour
{
    public event Action<GameObject, GameObject, float> OnObjectCollision;

    protected ProjectileInfo projectileInfo;
    protected Collider2D col;

    private void Awake() => col = GetComponent<Collider2D>();

    protected virtual void Start() => projectileInfo = GetComponent<ProjectileInfo>();

    protected virtual void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.isTrigger)
            return;

        if (_col.gameObject == gameObject)
            return;

        if (projectileInfo.TagsToIgnore.Contains(_col.gameObject.tag))
            return;

        OnObjectCollision?.Invoke(gameObject, _col.gameObject, projectileInfo.Damage);
    }
}