using UnityEngine;

[CreateAssetMenu(fileName = "Idle Random Wander In Circle & Chase When Player Visible", menuName = "Scriptable Objects/Enemy States/Idle States/Random Wander In Circle & Chase When Player Visible")]
public class EnemyIdleRandomPointInCircleChaseWhenPlayerVisible : EnemyIdleStateLogicBaseSO
{
    #region Wandering Modifiers
    [Header("Wandering Modifiers")]
    [SerializeField] private float minWanderRange;
    [SerializeField] private float maxWanderRange;

    [SerializeField] private float minWanderSpeed;
    [SerializeField] private float maxWanderSpeed;

    [SerializeField] private float minTimeBetweenWandering;
    [SerializeField] private float maxTimeBetweenWandering;

    [SerializeField] private float minDifferenceInX;
    [SerializeField] private float minDifferenceInY;
    [Space(15)]
    #endregion

    [Header("Raycast Fields")]
    [SerializeField] private int numRaycasts;
    [SerializeField] private float maxDistanceFromPlayerToChase;

    private bool isFirstFrame = true;

    private bool isDoneWandering = false;
    private bool isFirstFrameAfterWanderingComplete = true;
    private float waitTimer;

    public override void DoEnterStateLogic()
    {
        base.DoEnterStateLogic();
    }

    public override void DoExitStateLogic()
    {
        base.DoExitStateLogic();
    }

    public override void DoPhysicsUpdateStateLogic()
    {

    }

    public override void DoUpdateLogic()
    {
        if (IsPlayerVisible())
        {
            stateMachine.TransitionToState(stateMachine.ChaseState);
            return;
        }

        if (isFirstFrame)
        {
            enemyWeapon.ChangeWeaponLogic(idleStateAttackLogic, idleStateReloadLogic);

            isFirstFrame = false;
        }

        if (isDoneWandering)
        {
            int _maxAttempts = 4;
            int _attempts = 0;
            bool _successfulDestination = false;

            while (!_successfulDestination && _attempts < _maxAttempts)
            {
                Vector2 _movementVector = Random.insideUnitCircle;

                float _movementSpeed = Random.Range(minWanderSpeed, maxWanderSpeed);
                agent.speed = _movementSpeed;

                float _wanderRange = Random.Range(minWanderRange, maxWanderRange);
                _movementVector *= _wanderRange;

                Vector2 _finalPosition = (Vector2)trans.position + _movementVector;
                agent.SetDestination(_finalPosition);

                _successfulDestination = IsSuccessfulDestination(_finalPosition);

                // these ifs are to prevent the ai from hugging a wall (also having the effect of preventing enemy
                // from walking almost exactly straight west, east, north, and south)

                if (Mathf.Abs(trans.position.x - agent.destination.x) < minDifferenceInX)
                {
                    // MonoBehaviour.print("change in x is less than desired amount");

                    Vector2 _newMovementVector = _movementVector + new Vector2(1, 0);
                    _newMovementVector.Normalize();
                    _newMovementVector *= _wanderRange;

                    _finalPosition = (Vector2)trans.position + _newMovementVector;
                    agent.SetDestination(_finalPosition);

                    _successfulDestination = IsSuccessfulDestination(_finalPosition);

                    // MonoBehaviour.print("Is successful destination set in x: " + _successfulDestination);

                    if (!_successfulDestination)
                    {
                        _newMovementVector = _movementVector + new Vector2(-1, 0);
                        _newMovementVector.Normalize();
                        _newMovementVector *= _wanderRange;

                        _finalPosition = (Vector2)trans.position + _newMovementVector;
                        agent.SetDestination(_finalPosition);

                        _successfulDestination = IsSuccessfulDestination(_finalPosition);
                    }
                }

                else if (Mathf.Abs(trans.position.y - agent.destination.y) < minDifferenceInY)
                {
                    MonoBehaviour.print("change in y is less than desired amount");

                    Vector2 _newMovementVector = _movementVector + new Vector2(0, 1);
                    _newMovementVector.Normalize();
                    _newMovementVector *= _wanderRange;

                    _finalPosition = (Vector2)trans.position + _newMovementVector;
                    agent.SetDestination(_finalPosition);

                    _successfulDestination = IsSuccessfulDestination(_finalPosition);

                    MonoBehaviour.print("Is successful destination set in y: " + _successfulDestination);

                    if (!_successfulDestination)
                    {
                        _newMovementVector = _movementVector + new Vector2(0, -1);
                        _newMovementVector.Normalize();
                        _newMovementVector *= _wanderRange;

                        _finalPosition = (Vector2)trans.position + _newMovementVector;
                        agent.SetDestination(_finalPosition);

                        _successfulDestination = IsSuccessfulDestination(_finalPosition);
                    }
                }

                _attempts++;
            }

            isDoneWandering = false;
        }

        if (Mathf.RoundToInt(Vector2.Distance(trans.position, agent.destination)) <= agent.stoppingDistance)
        {
            if (isFirstFrameAfterWanderingComplete)
            {
                waitTimer = Random.Range(minTimeBetweenWandering, maxTimeBetweenWandering);
                isFirstFrameAfterWanderingComplete = false;
            }

            if (waitTimer <= 0f)
            {
                isDoneWandering = true;
                isFirstFrameAfterWanderingComplete = true;
            }

            waitTimer -= Time.deltaTime;
        }
    }

    protected override void ResetValues()
    {
        isDoneWandering = false;
        isFirstFrameAfterWanderingComplete = true;

        isFirstFrame = true;
    }

    private bool IsPlayerVisible()
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

    private bool IsSuccessfulDestination(Vector2 _finalPosition)
    {
        return (Vector2)agent.destination == _finalPosition;
    }
}