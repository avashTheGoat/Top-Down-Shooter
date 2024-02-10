using UnityEngine;

public class HammerWeapon : MeleeWeapon
{
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

        if (attackCooldownTimer != 0f) return;

        if (!attackLogic.ShouldAttack(this)) return;

        print("hammer attack");

        Attack();
        InvokeOnWeaponAttack();
        attackCooldownTimer = GetResetAttackTimer();
    }

    protected override void Attack()
    {
        base.Attack();
    }

    public Vector2 GetAttackAOECenter() => attackAOE.bounds.center;
}