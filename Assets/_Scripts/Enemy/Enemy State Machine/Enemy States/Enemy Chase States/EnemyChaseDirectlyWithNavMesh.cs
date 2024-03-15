using UnityEngine;

[CreateAssetMenu(fileName = "Chase Directly Using NavMesh", menuName = "Scriptable Objects/Enemy States/Chase States/Chase Directly Using NavMesh")]
public class EnemyChaseDirectlyWithNavMesh : EnemyChaseStateLogicBaseSO
{
    #region Chase Modifiers
    [Header("Chase Modifiers")]
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float minTimeToWanderAfterDetectionExit;
    [SerializeField] private float maxTimeToWanderAfterDetectionExit;
    [Space(15)]
    #endregion

    [Header("Raycast Fields")]
    [SerializeField] private int numRaycasts;
    [SerializeField] private float maxDistanceFromPlayerToChase;

    private bool isFirstFrame = true;

    private float timeToWanderAfterDetectionExit;
    private float timeAfterDetectionExit = 0f;
    private bool isFirstFrameAfterDetectionExit = true;

    public override void DoEnterStateLogic()
    {
        base.DoEnterStateLogic();

        agent.speed = chaseSpeed;
    }

    public override void DoPhysicsUpdateStateLogic()
    {
        throw new System.NotImplementedException();
    }

    public override void DoUpdateLogic()
    {
        if (ShouldBeInAttackState())
        {
            stateMachine.TransitionToState(stateMachine.AttackState);
            return;
        }

        if (isFirstFrame)
        {
            enemyWeapon.ChangeWeaponLogic(chaseStateAttackLogic, chaseStateReloadLogic);
            isFirstFrame = false;
        }

        Vector2 _playerPosition = Vector2.zero;
        if (player != null) _playerPosition = player.position;

        if (IsPlayerCloseEnough())
        {
            timeAfterDetectionExit = 0f;
            isFirstFrameAfterDetectionExit = true;
        }

        else
        {
            if (isFirstFrameAfterDetectionExit)
            {
                timeToWanderAfterDetectionExit = Random.Range(minTimeToWanderAfterDetectionExit, maxTimeToWanderAfterDetectionExit);
                timeAfterDetectionExit = 0f;
                isFirstFrameAfterDetectionExit = false;
            }

            timeAfterDetectionExit += Time.deltaTime;

            if (timeAfterDetectionExit >= timeToWanderAfterDetectionExit)
            {
                stateMachine.TransitionToState(stateMachine.IdleState);
                return;
            }
        }

        agent.SetDestination(_playerPosition);
    }

    protected override void ResetValues()
    {
        isFirstFrameAfterDetectionExit = true;
        timeAfterDetectionExit = 0f;

        isFirstFrame = true;
    }

    private bool IsPlayerCloseEnough()
    {
        Physics2D.queriesHitTriggers = false;
        Vector2 _raycastDirection = (Vector2)agent.velocity == Vector2.zero ? Vector2.right : agent.velocity;

        for (int i = 0; i < numRaycasts; i++)
        {
            RaycastHit2D[] _raycastHits = Physics2D.RaycastAll(trans.position, _raycastDirection, maxDistanceFromPlayerToChase + Mathf.Epsilon);
            _raycastDirection = Quaternion.Euler(0, 0, 360 / numRaycasts) * _raycastDirection;

            if (_raycastHits.Length <= 1) continue;

            // doing 1 because 0 is the enemy, so 1 is the closest object
            if (_raycastHits[1].collider.gameObject.transform == player && Vector2.Distance(_raycastHits[1].point, trans.position) <= maxDistanceFromPlayerToChase)
            {
                Physics2D.queriesHitTriggers = true;
                return true;
            }
        }

        Physics2D.queriesHitTriggers = true;
        return false;
    }
}