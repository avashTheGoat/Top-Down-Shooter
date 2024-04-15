using UnityEngine;

public class HammerWeapon : MeleeWeapon
{
    protected override void Update()
    {
        // TODO
        // remove these logical conditions later and
        // do a Debug.Log if Wielder or attackLogic == null
        // Doing this because i'm testing enemies spawning with weapons
        // right now
        if (Wielder == null || attackLogic == null)
        {
            print("Wielder or attackLogic == null");
            return;
        }

        base.Update();

        if (attackCooldownTimer != 0f) return;

        if (!attackLogic.ShouldAttack(this)) return;

        Attack();
        InvokeOnWeaponAttack();
        attackCooldownTimer = GetResetAttackTimer();
    }

    protected override void Attack()
    {
        base.Attack();
    }

    public Vector2 GetAttackAOECenter() => AttackAOE.bounds.center;
}