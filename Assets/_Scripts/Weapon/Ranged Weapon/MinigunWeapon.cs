using UnityEngine;

public class MinigunWeapon : RangedWeapon
{
    [Header("Shooting Angle")]
    [Tooltip("Minimum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    [SerializeField] private float minAngleChange;
    [Tooltip("Maximum change in angle for bullet shot. Should not be negative because that is applied randomly at runtime. Leave min and max at 0 for no angle change.")]
    [Min(0f)]
    [SerializeField] private float maxAngleChange;

    protected override void Update()
    {
        // TODO
        // remove these logical conditions later and
        // do a Debug.Log if Wielder or attackLogic is null
        // Doing this because i'm testing enemies spawning with weapons
        // right now
        if (Wielder is null || attackLogic is null)
        {
            print("Wielder or attackLogic is null");
            return;
        }

        base.Update();

        if (reloadLogic.ShouldReload(this) && reloadTimer == 0f)
        {
            Reload();
            attackCooldownTimer = GetResetAttackTimer();
            reloadTimer = RELOAD_TIME;
            InvokeOnWeaponReload();
            return;
        }

        if (reloadTimer != 0f) return;

        if (attackCooldownTimer != 0f) return;

        if (!attackLogic.ShouldAttack(this)) return;

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

        float _randAngleChange = Random.Range(minAngleChange, maxAngleChange);
        _randAngleChange = Random.Range(0, 1 + 1) == 1 ? -_randAngleChange : _randAngleChange;

        Projectile _bullet = Instantiate(projectile);
        ShotProjectiles.Add(_bullet);
        _bullet.Init(weaponDamage, projectileSpeed, trans.position, trans.localEulerAngles.z + _randAngleChange, projectileRange);
    }
}