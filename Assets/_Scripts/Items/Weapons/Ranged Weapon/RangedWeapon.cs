using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public event Action<GameObject> OnAttackWithoutAmmo;
    public event Action<GameObject> OnReload;
    public event Action<GameObject> OnReloadComplete;

    public float ReloadTime => reloadTime;
    public float MaxAmmo => maxAmmo;

    public float ProjectileSpeed => projectileSpeed;
    public float Range => range;

    public float MinAngleChange => minAngleChange;
    public float MaxAngleChange => maxAngleChange;

    public float TotalDeltaAngle => totalDeltaAngle;
    public int NumProjectiles => numProjectiles;

    [Header("Reloading/Ammo")]
    [SerializeField] protected float reloadTime;
    [SerializeField] protected int maxAmmo;
    [Space(15)]

    [Header("Projectile")]
    [SerializeField] protected ProjectileInfo projectile;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float range;
    [Space(15)]

    [Header("Shooting Angle")]
    [Tooltip("Minimum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    [SerializeField] protected float minAngleChange;
    [Tooltip("Maximum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    [SerializeField] protected float maxAngleChange;

    [Header("Multi-Shot")]
    [Tooltip("Difference in angle between leftmost and rightmost bullets")]
    [Min(0f)]
    [SerializeField] protected float totalDeltaAngle = 0f;
    [Min(1)]
    [SerializeField] protected int numProjectiles = 1;

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

        ammo = maxAmmo;
        reloadTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();

        reloadTimer -= Time.deltaTime;
        reloadTimer = Mathf.Clamp(reloadTimer, 0, reloadTime);
    }

    protected abstract void Reload();

    public virtual void SetReloadTime(float _newReloadTime)
    {
        if (_newReloadTime < 0)
            throw new ArgumentException("Reload time cannot be negative.");

        reloadTime = _newReloadTime;
    }

    public virtual void SetMaxAmmo(int _newMaxAmmo)
    {
        if (_newMaxAmmo < 0)
            throw new ArgumentException("Max ammo cannot be negative.");

        maxAmmo = _newMaxAmmo;
    }

    public virtual void SetProjectileSpeed(float _newProjectileSpeed)
    {
        if (_newProjectileSpeed < 0)
            throw new ArgumentException("Projectile speed cannot be negative.");

        projectileSpeed = _newProjectileSpeed;
    }

    public virtual void SetRange(float _newRange)
    {
        if (_newRange < 0)
            throw new ArgumentException("Range cannot be negative.");

        range = _newRange;
    }

    public virtual void SetMinAngleChange(float _newMinAngleChange) => minAngleChange = _newMinAngleChange;

    public virtual void SetMaxAngleChange(float _newMaxAngleChange) => maxAngleChange = _newMaxAngleChange;

    public virtual void SetTotalDeltaAngle(float _newTotalDeltaAngle)
    {
        if (_newTotalDeltaAngle < 0)
            throw new ArgumentException("Total delta angle cannot be negative.");

        totalDeltaAngle = _newTotalDeltaAngle;
    }

    public virtual void SetNumProjectiles(int _newNumProjectiles)
    {
        if (_newNumProjectiles < 0)
            throw new ArgumentException("Number of projectiles cannot be negative.");

        numProjectiles = _newNumProjectiles;
    }

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
        float _randAngleChange = UnityEngine.Random.Range(minAngleChange, maxAngleChange);
        _randAngleChange = UnityEngine.Random.Range(0, 1 + 1) == 1 ? -_randAngleChange : _randAngleChange;

        return _randAngleChange;
    }

    protected void InvokeOnReload() => OnReload?.Invoke(gameObject);
    protected void InvokeOnReloadComplete() => OnReloadComplete?.Invoke(gameObject);
    protected void InvokeOnAttackWithoutAmmo() => OnAttackWithoutAmmo?.Invoke(gameObject);
}