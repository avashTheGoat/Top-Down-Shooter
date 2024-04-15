using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public event Action<GameObject> OnAttackWithoutAmmo;
    public event Action<GameObject> OnReload;
    public event Action<GameObject> OnReloadComplete;

    [Header("Reloading/Ammo")]
    public float ReloadTime;
    public int MaxAmmo;
    [Space(15)]

    [Header("Projectile")]
    [SerializeField] protected ProjectileInfo projectile;
    public float ProjectileSpeed;
    public float Range;
    [Space(15)]

    [Header("Shooting Angle")]
    [Tooltip("Minimum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    public float MinAngleChange;
    [Tooltip("Maximum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    public float MaxAngleChange;

    [Header("Multi-Shot")]
    [Tooltip("Difference in angle between leftmost and rightmost bullets")]
    [Min(0f)]
    public float TotalDeltaAngle = 0f;
    [Min(1)]
    public int NumProjectiles = 1;

    public float Ammo => ammo;
    public float CurReloadTimeLeft => reloadTimer;

    protected IReload reloadLogic;
    protected List<ProjectileInfo> shotProjectiles = new();
    
    protected int ammo;
    protected float reloadTimer;
    protected bool didReload = false;

    protected override void Awake()
    {
        base.Awake();

        ammo = MaxAmmo;
        reloadTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();

        reloadTimer -= Time.deltaTime;
        reloadTimer = Mathf.Clamp(reloadTimer, 0, ReloadTime);
    }

    protected abstract void Reload();

    public void SetWeaponLogic(IAttack _attackLogic, IReload _reloadLogic)
    {
        SetWeaponLogic(_attackLogic);

        if (_reloadLogic == null)
            throw new ArgumentNullException(nameof(_reloadLogic), "_reloadLogic cannot be null for a RangedWeapon.");

        reloadLogic = _reloadLogic;
    }
    
    public override void ResetWeapon()
    {
        reloadTimer = 0f;
        didReload = false;
    }
    
    public List<ProjectileInfo> GetShotProjectiles()
    {
        shotProjectiles.RemoveAll(_projectile => _projectile == null);
        return shotProjectiles;
    }

    protected float GetRandAngleChange()
    {
        float _randAngleChange = UnityEngine.Random.Range(MinAngleChange, MaxAngleChange);
        _randAngleChange = UnityEngine.Random.Range(0, 1 + 1) == 1 ? -_randAngleChange : _randAngleChange;

        return _randAngleChange;
    }

    protected void InvokeOnReload() => OnReload?.Invoke(gameObject);
    protected void InvokeOnReloadComplete() => OnReloadComplete?.Invoke(gameObject);
    protected void InvokeOnAttackWithoutAmmo() => OnAttackWithoutAmmo?.Invoke(gameObject);
}