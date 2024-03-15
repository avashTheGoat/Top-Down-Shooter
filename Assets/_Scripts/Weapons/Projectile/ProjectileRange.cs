using System;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class ProjectileRange : MonoBehaviour
{
    public event Action OnProjectileMaxRange;

    private float range;
    private float distanceTraveled;
    private Transform trans;
    private Vector2 startPosition;
    private bool hasBulletReachedMax = false;

    private void Awake()
    {
        trans = transform;
    }

    private void Start()
    {
        range = GetComponent<Projectile>().Range;
        startPosition = trans.position;
    }

    private void Update()
    {
        distanceTraveled = Vector2.Distance(startPosition, trans.position);

        if (distanceTraveled >= range && !hasBulletReachedMax)
        {
            OnProjectileMaxRange?.Invoke();
            hasBulletReachedMax = true;
        }
    }
}