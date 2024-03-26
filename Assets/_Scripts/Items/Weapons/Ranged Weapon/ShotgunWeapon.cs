using UnityEngine;

public class ShotgunWeapon : RangedWeapon
{
    [Header("Multi-Shot")]
    [Tooltip("Difference in angle between leftmost and rightmost bullets")]
    [SerializeField] private float totalDeltaAngle;
    [SerializeField] private int numBullets;

    protected override void Update()
    {
        base.Update();

        if (reloadLogic.ShouldReload(this) && reloadTimer == 0f)
        {
            InvokeOnWeaponReload();

            attackCooldownTimer = GetResetAttackTimer();
            reloadTimer = RELOAD_TIME;
            didReload = true;

            return;
        }

        if (reloadTimer != 0f)
            return;

        if (didReload)
        {
            Reload();
            didReload = false;
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

        float _deltaAngle = totalDeltaAngle / 2;
        for (int i = 0; i < numBullets; i++)
        {
            Projectile _bullet = Instantiate(projectile);
            shotProjectiles.Add(_bullet);
            _bullet.Init(weaponDamage, projectileSpeed, trans.position, trans.localEulerAngles.z + _deltaAngle, projectileRange);

            _deltaAngle -= totalDeltaAngle / (numBullets - 1);
        }
    }
}