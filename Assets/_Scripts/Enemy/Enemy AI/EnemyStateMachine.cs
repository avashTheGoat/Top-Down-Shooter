using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreLayers;

    [Header("State Logic")]
    [SerializeField] private EnemyIdleStateLogicBaseSO idleStateLogic;
    [SerializeField] private EnemyChaseStateLogicBaseSO chaseStateLogic;
    [SerializeField] private EnemyAttackStateLogicBaseSO attackStateLogic;
    [Space(15)]

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite leftMovingSprite;
    [SerializeField] private Sprite rightMovingSprite;

    #region States
    public BaseState CurrentState { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    #endregion

    private NavMeshAgent agent;
    private Transform player;
    private Weapon weapon;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        player = PlayerProvider.GetPlayer();

        IdleState = new EnemyIdleState(Instantiate(idleStateLogic)
            .Initialize(this, transform, agent, player, weapon, spriteRenderer, leftMovingSprite, rightMovingSprite));

        ChaseState = new EnemyChaseState(Instantiate(chaseStateLogic)
            .Initialize(this, transform, agent, player, ignoreLayers, weapon, spriteRenderer,
            leftMovingSprite, rightMovingSprite));

        AttackState = new EnemyAttackState(Instantiate(attackStateLogic)
            .Initialize(this, transform, agent, ignoreLayers, player, weapon, spriteRenderer,
            leftMovingSprite, rightMovingSprite));

        CurrentState = IdleState;
        IdleState.EnterState();
    }

    private void Update() => CurrentState.UpdateState();

    public void TransitionToState(BaseState _newState)
    {
        CurrentState.ExitState();
        CurrentState = _newState;
        CurrentState.EnterState();
    }

    public void Init(Weapon _weapon) => weapon = _weapon;
}