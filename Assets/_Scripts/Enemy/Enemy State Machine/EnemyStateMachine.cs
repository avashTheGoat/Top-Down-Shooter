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
    [Space(15)]

    [Header("State Logic")]
    [SerializeField] private EnemyIdleStateLogicBaseSO idleStateLogic;
    [SerializeField] private EnemyChaseStateLogicBaseSO chaseStateLogic;
    [SerializeField] private EnemyAttackStateLogicBaseSO attackStateLogic;
    [Space(15)]

    // very iffy to use Component, but i'm doing it so that it is visible in the inspector
    // just IAttack and IReload will not be visible in the inspector
    [Header("Attacking Logic")]
    [SerializeField] private Component idleStateAttack;
    [SerializeField] private Component chaseStateAttack;
    [SerializeField] private Component attackStateAttack;
    #nullable enable
    [SerializeField] private Component? idleStateReload;
    [SerializeField] private Component? chaseStateReload;
    [SerializeField] private Component? attackStateReload;
    #nullable disable

    #region States
    private BaseState currentState;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    #endregion

    private NavMeshAgent agent;
    private Transform player;
    private Weapon weapon;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerProvider.GetPlayer();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        IdleState = new EnemyIdleState(Instantiate(idleStateLogic).Initialize(this, transform, agent, player, weapon,
            (IAttack)idleStateAttack, (IReload)idleStateReload));
        ChaseState = new EnemyChaseState(Instantiate(chaseStateLogic).Initialize(this, transform, agent, player, weapon,
            (IAttack)chaseStateAttack, (IReload)chaseStateReload));
        AttackState = new EnemyAttackState(Instantiate(attackStateLogic).Initialize(this, transform, agent, player,
            weapon, (IAttack)attackStateAttack, (IReload)attackStateReload));

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

    private void Update() => currentState.UpdateState();

    public void TransitionToState(BaseState _newState)
    {
        currentState.ExitState();
        currentState = _newState;
        currentState.EnterState();
    }

    public void Init(Weapon _weapon) => weapon = _weapon;
}