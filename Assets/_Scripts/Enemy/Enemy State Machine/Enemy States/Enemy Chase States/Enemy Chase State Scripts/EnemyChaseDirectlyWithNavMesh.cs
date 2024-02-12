using UnityEngine;

[CreateAssetMenu(fileName = "Chase Directly Using NavMesh", menuName = "Scriptable Objects/Enemy States/Chase States/Chase Directly Using NavMesh")]
public class EnemyChaseDirectlyWithNavMesh : EnemyChaseStateLogicBaseSO
{
    #region Chase Modifiers
    [Header("Chase Modifiers")]
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float minTimeToWanderAfterDetectionExit;
    [SerializeField] private float maxTimeToWanderAfterDetectionExit;
    [SerializeField] private float timeToWanderAfterDetectionExit;
    #endregion

    private bool isFirstFrame = true;

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

        Vector2 _playerPosition = new();
        if (player != null) _playerPosition = player.position;

        bool _isPlayerOutOfDetection = !IsPointInCollider(detectionCollider, _playerPosition);

        if (_isPlayerOutOfDetection)
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

        //player is in detection and player is not in attack
        else
        {
            timeAfterDetectionExit = 0f;
            isFirstFrameAfterDetectionExit = true;
        }

        agent.SetDestination(_playerPosition);
    }

    protected override void ResetValues()
    {
        isFirstFrameAfterDetectionExit = true;
        timeAfterDetectionExit = 0f;

        isFirstFrame = true;
}
}