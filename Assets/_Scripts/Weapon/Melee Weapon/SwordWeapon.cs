public class SwordWeapon : MeleeWeapon
{
    protected override void Update()
    {
        // TODO
        // remove these logical conditions later and
        // do a Debug.Log if Wielder or attackLogic is null
        // Doing this because i'm testing enemies spawning with weapons
        // right now
        if (Wielder is null || AttackLogic is null)
        {
            print("Wielder or attackLogic is null");
            return;
        }

        base.Update();

        if (attackCooldownTimer != 0f) return;

        if (!AttackLogic.ShouldAttack(this)) return;

        Attack();
        InvokeOnWeaponAttack();
        attackCooldownTimer = GetResetAttackTimer();
    }

    protected override void Attack()
    {
        base.Attack();
    }
}