public class PistolWeapon : RangedWeapon
{
    protected override void Update()
    {
        // TODO
        // remove these logical conditions later and
        // do a Debug.Log if Wielder or AttackLogic is null
        // Doing this because i'm testing enemies spawning with weapons
        // right now
        if (Wielder is null || AttackLogic is null)
        {
            print("Wielder or AttackLogic is null");
            return;
        }

        base.Update();

        if (ReloadLogic.ShouldReload(this) && reloadTimer == 0f)
        {
            Reload();
            attackCooldownTimer = GetResetAttackTimer();
            reloadTimer = RELOAD_TIME;
            InvokeOnWeaponReload();
            return;
        }

        if (reloadTimer != 0f) return;

        if (attackCooldownTimer != 0f) return;

        if (!AttackLogic.ShouldAttack(this)) return;

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

    protected override void Reload()
    {
        ammo = maxAmmo;
    }

    protected override void Attack()
    {
        base.Attack();

        Projectile _bullet = Instantiate(projectile);
        ShotProjectiles.Add(_bullet);
        _bullet.Init(weaponDamage, projectileSpeed, trans.position, trans.localEulerAngles.z, projectileRange);
    }
}