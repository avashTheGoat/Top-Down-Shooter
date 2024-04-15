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

        MAX_DAMAGE = Damage;
        MAX_PROJECTILE_SPEED = ProjectileSpeed;
        MAX_RANGE = Range;
    }

    protected override void Update()
    {
        base.Update();

        if (reloadLogic.ShouldReload(this) && reloadTimer == 0f)
        {
            InvokeOnReload();
            
            attackCooldownTimer = GetResetAttackTimer();
            reloadTimer = ReloadTime;
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

                Damage = MAX_DAMAGE * maxDamagePercentCurve.Evaluate(chargeTimer);
                ProjectileSpeed = MAX_PROJECTILE_SPEED * maxProjectileSpeedPercentCurve.Evaluate(chargeTimer);
                Range = MAX_RANGE * maxRangePercentCurve.Evaluate(chargeTimer);

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

    protected override void Reload() => ammo = MaxAmmo;
    
    protected override void Attack()
    {
        base.Attack();

        float _deltaAngle = GetRandAngleChange();

        ProjectileInfo _arrow = Instantiate(projectile);
        _arrow.Init(Damage, ProjectileSpeed, trans.position,
        trans.localEulerAngles.z + _deltaAngle, Range, TagsToIgnore);

        shotProjectiles.Add(_arrow);
    }
}