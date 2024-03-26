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
    protected bool didReload = false;

    protected List<string> tagsToIgnore;

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

    protected abstract void Reload();

    public void SetWeaponLogic(IAttack _attackLogic, IReload _reloadLogic)
    {
        SetWeaponLogic(_attackLogic);

        if (_reloadLogic == null)
            throw new ArgumentNullException(nameof(_reloadLogic), "_reloadLogic cannot be null for a RangedWeapon.");

        reloadLogic = _reloadLogic;
    }

    public void SetTagsToIgnore(List<string> _tagsToIgnore)
    {
        if (_tagsToIgnore == null)
            throw new ArgumentNullException(nameof(_tagsToIgnore), $"{nameof(_tagsToIgnore)} should not be null.");

        tagsToIgnore = _tagsToIgnore;
    }

    public List<Projectile> GetShotProjectiles()
    {
        shotProjectiles.RemoveAll(_projectile => _projectile == null);
        return shotProjectiles;
    }

    protected void InvokeOnWeaponReload() => OnWeaponReload?.Invoke();
    protected void InvokeOnAttackWithoutAmmo() => OnAttackWithoutAmmo?.Invoke();
}