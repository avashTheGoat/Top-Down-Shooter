public class PistolWeapon : RangedWeapon
{
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
            return;

        if (ammo <= 0)
        {
            InvokeOnAttackWithoutAmmo();
            return;
        }

        Attack();
        ammo--;
        InvokeOnWeaponAttack();
        attackCooldownTimer = GetResetAttackTimer();
    }

    protected override void Reload() => ammo = MaxAmmo;

    protected override void Attack()
    {
        base.Attack();

        float _deltaAngle = GetRandAngleChange();

        ProjectileInfo _bullet = Instantiate(projectile);
        shotProjectiles.Add(_bullet);
        _bullet.Init(Damage, ProjectileSpeed, trans.position,
        trans.localEulerAngles.z + _deltaAngle, Range, TagsToIgnore);
    }
}