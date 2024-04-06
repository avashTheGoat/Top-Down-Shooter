using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public event Action<GameObject> OnAttackWithoutAmmo;
    public event Action OnReload;
    public event Action OnReloadComplete;

    public float Ammo => ammo;
    public float MaxAmmo => maxAmmo;

    public float Range => projectileRange;

    public float ReloadTime => RELOAD_TIME;
    public float CurReloadTimeLeft => reloadTimer;

    [Header("Reloading/Ammo")]
    [SerializeField] protected float RELOAD_TIME;
    [SerializeField] protected int maxAmmo;
    [Space(15)]

    [Header("Projectile")]
    [SerializeField] protected ProjectileInfo projectile;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileRange;
    [Space(15)]

    [Header("Shooting Angle")]
    [Tooltip("Minimum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    [SerializeField] protected float minAngleChange;
    [Tooltip("Maximum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    [SerializeField] protected float maxAngleChange;

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
        reloadTimer = Mathf.Clamp(reloadTimer, 0, RELOAD_TIME);
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
        float _randAngleChange = UnityEngine.Random.Range(minAngleChange, maxAngleChange);
        _randAngleChange = UnityEngine.Random.Range(0, 1 + 1) == 1 ? -_randAngleChange : _randAngleChange;

        return _randAngleChange;
    }

    protected void InvokeOnReload() => OnReload?.Invoke();
    protected void InvokeOnReloadComplete() => OnReloadComplete?.Invoke();
    protected void InvokeOnAttackWithoutAmmo() => OnAttackWithoutAmmo?.Invoke(gameObject);
}