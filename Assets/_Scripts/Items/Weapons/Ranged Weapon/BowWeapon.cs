using System;
using UnityEngine;

public class BowWeapon : RangedWeapon
{
    public event Action<float, float> OnBowCharge;

    [Header("Charging")]
    [SerializeField] private float maxChargeTime;
    [SerializeField] private float minChargeTimeToShoot;
    [Tooltip("Curve describing what percentage of the damage variable the arrow will have. As the x increases, time charging the arrow increases.")]
    [SerializeField] private AnimationCurve maxDamagePercentCurve;
    [Tooltip("Curve describing what percentage of the speed variable the arrow will have. As the x increases, time charging the arrow increases.")]
    [SerializeField] private AnimationCurve maxProjectileSpeedPercentCurve;
    [Tooltip("Curve describing what percentage of the range variable the arrow will have. As the x increases, time charging the arrow increases.")]
    [SerializeField] private AnimationCurve maxRangePercentCurve;

    private float chargeTimer = 0f;

    private float MAX_DAMAGE;
    private float MAX_PROJECTILE_SPEED;
    private float MAX_RANGE;

    private bool hasChargedPreviously;

    protected override void Awake()
    {
        base.Awake();

        MAX_DAMAGE = damage;
        MAX_PROJECTILE_SPEED = projectileSpeed;
        MAX_RANGE = Range;
    }

    protected override void Update()
    {
        base.Update();

        if (reloadLogic.ShouldReload(this) && reloadTimer == 0f)
        {
            InvokeOnReload();
            
            attackCooldownTimer = GetResetAttackTimer();
            reloadTimer = reloadTime;
            didReload = true;
            
            return;
        }

        if (reloadTimer != 0f)
            return;

        if (didReload)
        {
            Reload();
            didReload = false;

            InvokeOnReloadComplete();
        }

        if (attackCooldownTimer != 0f)
            return;

        if (!attackLogic.ShouldAttack(this))
        {
            if (hasChargedPreviously && ammo > 0)
            {
                if (chargeTimer < minChargeTimeToShoot)
                {
                    hasChargedPreviously = false;
                    InvokeOnAttackWithoutAmmo();
                    return;
                }

                chargeTimer = Mathf.Clamp(chargeTimer / maxChargeTime, 0f, 1f);

                damage = MAX_DAMAGE * maxDamagePercentCurve.Evaluate(chargeTimer);
                projectileSpeed = MAX_PROJECTILE_SPEED * maxProjectileSpeedPercentCurve.Evaluate(chargeTimer);
                range = MAX_RANGE * maxRangePercentCurve.Evaluate(chargeTimer);

                Attack();

                ammo--;
                InvokeOnWeaponAttack();

                attackCooldownTimer = GetResetAttackTimer();
                chargeTimer = 0f;

                hasChargedPreviously = false;
            }

            return;
        }

        hasChargedPreviously = true;

        if (ammo <= 0)
        {
            InvokeOnAttackWithoutAmmo();
            return;
        }

        chargeTimer += Time.deltaTime;
        OnBowCharge?.Invoke(maxChargeTime, chargeTimer);
    }

    public override void SetDamage(float _newDamage)
    {
        base.SetDamage(_newDamage);
        MAX_DAMAGE = _newDamage;
    }

    public override void SetProjectileSpeed(float _newProjectileSpeed)
    {
        base.SetProjectileSpeed(_newProjectileSpeed);
        MAX_PROJECTILE_SPEED = _newProjectileSpeed;
    }

    public override void SetRange(float _newRange)
    {
        base.SetRange(_newRange);
        MAX_RANGE = _newRange;
    }

    protected override void Reload() => ammo = maxAmmo;
    
    protected override void Attack()
    {
        base.Attack();

        float _deltaAngle = GetRandAngleChange();

        ProjectileInfo _arrow = Instantiate(projectile);
        _arrow.Init(damage, projectileSpeed, trans.position,
        trans.localEulerAngles.z + _deltaAngle, Range, TagsToIgnore);

        shotProjectiles.Add(_arrow);
    }
}