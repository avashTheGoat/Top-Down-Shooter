using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    private enum EnemyState
    {
        Wander,
        Chase,
        Attack
    };

    [SerializeField] private EnemyState startingState;

    [Header("Enemy Colliders")]
    [SerializeField] private Collider2D detectionCollider;
    [SerializeField] private Collider2D attackStartCollider;
    [SerializeField] private Collider2D attackAOE;
    [Space(15)]

    [Header("State Logic")]
    [SerializeField] private EnemyIdleStateLogicBaseSO idleStateLogic;
    [SerializeField] private EnemyChaseStateLogicBaseSO chaseStateLogic;
    [SerializeField] private EnemyAttackStateLogicBaseSO attackStateLogic;
    [Space(15)]

    #region States
    private BaseState currentState;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    #endregion

    private NavMeshAgent agent;

    private Transform player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerProvider.GetPlayer();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        IdleState = new EnemyIdleState(Instantiate(idleStateLogic).Initialize(this, transform, agent, player, detectionCollider));
        ChaseState = new EnemyChaseState(Instantiate(chaseStateLogic).Initialize(this, transform, agent, player, detectionCollider, attackStartCollider));
        AttackState = new EnemyAttackState(Instantiate(attackStateLogic).Initialize(this, transform, agent, player, attackStartCollider, attackAOE));

        switch (startingState)
        {
            case EnemyState.Wander:
                currentState = IdleState;
                break;

            case EnemyState.Chase:
                currentState = ChaseState;
                break;

            case EnemyState.Attack:
                currentState = AttackState;
                break;
        }
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void TransitionToState(BaseState _newState)
    {
        currentState.ExitState();
        currentState = _newState;
        currentState.EnterState();
    }
}