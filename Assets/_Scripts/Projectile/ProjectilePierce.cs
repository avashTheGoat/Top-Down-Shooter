using UnityEngine;

public class ProjectilePierce : ProjectileHit
{
    [HideInInspector]
    public int PierceCounter = 0;

    [field: SerializeField] public int MaxPierces { get; private set; }
}