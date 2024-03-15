using UnityEngine;

[CreateAssetMenu(fileName = "Attack & Chase Visible Player", menuName = "Scriptable Objects/Enemy States/Attack States/Attack & Chase Visible Player")]
public class EnemyAttackChaseVisiblePlayer : EnemyAttackStateLogicBaseSO
{
    [Header("Raycast Fields")]
    [Tooltip("The number of raycasts that should be created to check for player visibility")]
    [SerializeField] private int numRaycasts;
    [SerializeField] private float maxRaycastDistance;

    private bool isFirstFrame = true;

    private float initialSpeed;

    public override void DoEnterStateLogic()
    {
        base.DoEnterStateLogic();

        initialSpeed = agent.speed;
    }

    public override void DoPhysicsUpdateStateLogic()
    {

    }

    public override void DoUpdateLogic()
    {
        if (!ShouldBeInAttackState() || !IsPlayerVisible())
        {
            stateMachine.TransitionToState(stateMachine.ChaseState);
            return;
        }

        if (isFirstFrame)
        {
            enemyWeapon.ChangeWeaponLogic(attackingStateAttackLogic, attackingStateReloadLogic);
            agent.speed = initialSpeed * attackingSpeedPercent;

            isFirstFrame = false;
        }

        agent.SetDestination(player.position);
    }

    protected override void ResetValues()
    {
        isFirstFrame = true;
    }

    public override void DoExitStateLogic()
    {
        base.DoExitStateLogic();

        agent.speed = initialSpeed;
    }

    private bool IsPlayerVisible()
    {
        Physics2D.queriesHitTriggers = false;
        Vector2 _raycastDirection = (Vector2)agent.velocity == Vector2.zero ? Vector2.right : agent.velocity;

        for (int i = 0; i < numRaycasts; i++)
        {
            RaycastHit2D[] _raycastHits = Physics2D.RaycastAll(trans.position, _raycastDirection, maxRaycastDistance + Mathf.Epsilon);
            _raycastDirection = Quaternion.Euler(0, 0, 360 / numRaycasts) * _raycastDirection;

            if (_raycastHits.Length <= 1) continue;

            // doing 1 because 0 is the enemy, so 1 is the closest object
            if (_raycastHits[1].collider.gameObject.transform == player)
            {
                Physics2D.queriesHitTriggers = true;
                return true;
            }
        }

        Physics2D.queriesHitTriggers = true;
        // MonoBehaviour.print("Cannot see player");
        return false;
    }
}