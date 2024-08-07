public class PistolWeapon : RangedWeapon
{
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

    protected override void Reload() => ammo = maxAmmo;

    protected override void Attack()
    {
        base.Attack();

        float _deltaAngle = GetRandAngleChange();

        ProjectileInfo _bullet = Instantiate(projectile);
        shotProjectiles.Add(_bullet);
        _bullet.Init(damage, projectileSpeed, trans.position,
        trans.localEulerAngles.z + _deltaAngle, range, TagsToIgnore);
    }
}