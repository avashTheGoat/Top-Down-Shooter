using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Static AOE Melee Attack", menuName = "Scriptable Objects/Enemy States/Attack States/Static AOE Melee")]
public class EnemyAttackMeleeStaticAOE : EnemyAttackStateLogicBaseSO
{
    #region Attack Modifiers
    [SerializeField] private float attackDamage;
    [SerializeField] private float attacksPerSecond;
    [Header("Attack Effects")]
    [Tooltip("If on, the enemy can damage damageable game objects other than the player, including other enemies.")]
    [SerializeField] private bool canDamageEnvironment;
    #endregion

    private bool canAttack = true;
    private bool isFirstFrameAfterAttacking = true;
    private float attackTimer = 0f;

    public override void DoPhysicsUpdateStateLogic()
    {
        
    }

    public override void DoUpdateLogic()
    {
        if (!IsPointInCollider(attackCollider, player.position))
        {
            MonoBehaviour.print("Switching to chase state from attack state");
            stateMachine.TransitionToState(stateMachine.ChaseState);
            return;
        }

        if (canAttack)
        {
            canAttack = false;

            List<Collider2D> _hits = new List<Collider2D>();
            if (canDamageEnvironment)
            {
                attackAOE.GetContacts(_hits);
            }

            else
            {
                ContactFilter2D _contactFilter = new();
                LayerMask _mask = player.gameObject.layer;
                _contactFilter.SetLayerMask(_mask);

                attackAOE.GetContacts(_contactFilter, _hits);
            }

            MonoBehaviour.print("Enemy attacked!");
        }

        if (!canAttack)
        {
            if (isFirstFrameAfterAttacking)
            {
                isFirstFrameAfterAttacking = false;
                attackTimer = 1 / attacksPerSecond;
                return;
            }

            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                canAttack = true;
                isFirstFrameAfterAttacking = true;
            }
        }
    }

    protected override void ResetValues()
    {
        canAttack = true;
        isFirstFrameAfterAttacking = true;
        attackTimer = 0f;
    }
}