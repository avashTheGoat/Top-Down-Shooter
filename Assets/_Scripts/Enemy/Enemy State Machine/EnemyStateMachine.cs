using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreLayers;

    [Header("State Logic")]
    [SerializeField] private EnemyIdleStateLogicBaseSO idleStateLogic;
    [SerializeField] private EnemyChaseStateLogicBaseSO chaseStateLogic;
    [SerializeField] private EnemyAttackStateLogicBaseSO attackStateLogic;

    #region States
    public BaseState CurrentState { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    #endregion

    private NavMeshAgent agent;
    private Transform player;
    private Weapon weapon;

    private Component enemyWaves;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerProvider.GetPlayer();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        IdleState = new EnemyIdleState(Instantiate(idleStateLogic).Initialize(this, transform, agent, player,
        ((EnemyWavesSpawner)enemyWaves).SpawnedEnemies, weapon));

        ChaseState = new EnemyChaseState(Instantiate(chaseStateLogic)
        .Initialize(this, transform, agent, player, ignoreLayers, weapon));

        AttackState = new EnemyAttackState(Instantiate(attackStateLogic)
        .Initialize(this, transform, agent, ignoreLayers, player, weapon));

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

    public void Init(Weapon _weapon, EnemyWavesSpawner _enemyWaves)
    {
        weapon = _weapon;
        enemyWaves = _enemyWaves;
    }
}