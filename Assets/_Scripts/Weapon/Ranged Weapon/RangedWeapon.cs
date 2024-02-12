using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public event Action OnAttackWithoutAmmo;
    public event Action OnWeaponReload;



    public float Ammo => ammo;
    public float MaxAmmo => maxAmmo;
    public float Range => projectileRange;

    [Header("Reloading")]
    [SerializeField] protected float RELOAD_TIME;
    [Space(15)]

    [Header("Projectile")]
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileRange;
    [Space(15)]    

    [SerializeField] protected int maxAmmo;

    protected IReload reloadLogic;
    protected List<Projectile> shotProjectiles = new();
    
    protected int ammo;
    protected float reloadTimer;

    protected override void Awake()
    {
        base.Awake();

        ammo = maxAmmo;
        reloadTimer = 0;
    }

    protected override void Update()
    {
        base.Update();

        reloadTimer -= Time.deltaTime;
        reloadTimer = Mathf.Clamp(reloadTimer, 0, RELOAD_TIME);
    }

    public override void Init(Transform _wielder, IAttack _attackLogic)
    {
        throw new AccessViolationException("This Init function should not be used for RangedWeapon objects.");
    }

    public void Init(Transform _wielder, IAttack _attackLogic, IReload _reloadLogic)
    {
        if (_reloadLogic == null) throw new ArgumentNullException(nameof(_reloadLogic), "The passed in IReload should not be null.");

        base.Init(_wielder, _attackLogic);
        reloadLogic = _reloadLogic;
    }

    protected abstract void Reload();

    public List<Projectile> GetShotProjectiles()
    {
        shotProjectiles.RemoveAll(_projectile => _projectile == null);
        return shotProjectiles;
    }

    protected void InvokeOnWeaponReload() => OnWeaponReload?.Invoke();
    protected void InvokeOnAttackWithoutAmmo() => OnAttackWithoutAmmo?.Invoke();
}