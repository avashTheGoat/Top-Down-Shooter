using UnityEngine;
using System;

[RequireComponent(typeof(ProjectileInfo), typeof(Collider2D))]
public class ProjectileHit : MonoBehaviour
{
    public event Action<GameObject, GameObject, float> OnObjectCollision;

    [SerializeField] protected LayerMask layersToIgnore;

    protected ProjectileInfo projectileInfo;
    protected Collider2D col;

    protected Transform trans;
    protected Vector2 prevPos;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        trans = transform;
    }

    protected virtual void Start()
    {
        projectileInfo = GetComponent<ProjectileInfo>();
        prevPos = projectileInfo.StartPosition;
    }

    private void FixedUpdate()
    {
        bool _initialSetting = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;

        Vector2 _rayDir = (prevPos - (Vector2)trans.position).normalized;
        float _rayLength = Vector2.Distance(trans.position, prevPos);

        RaycastHit2D _hit = Physics2D.Raycast(prevPos, _rayDir, _rayLength, ~layersToIgnore);
        if (_hit && !projectileInfo.TagsToIgnore.Contains(_hit.collider.gameObject.tag))
        {
            print(_hit.collider.gameObject);
            OnObjectCollision?.Invoke(gameObject, _hit.collider.gameObject, projectileInfo.Damage);
        }

        prevPos = trans.position;
        Physics2D.queriesHitTriggers = _initialSetting;
    }
}