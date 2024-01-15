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

    private float timeAfterDetectionExit;
    private bool isFirstFrameAfterDetectionExit = true;
    private bool isPlayerOutOfDetection = false;

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
        isPlayerOutOfDetection = !IsPointInCollider(detectionCollider, player.position);

        if (isPlayerOutOfDetection)
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

        else if (IsPointInCollider(attackCollider, player.position))
        {
            stateMachine.TransitionToState(stateMachine.AttackState);
            return;
        }

        //player is in detection and player is not in attack
        else
        {
            timeAfterDetectionExit = 0f;
            isFirstFrameAfterDetectionExit = true;
        }

        agent.SetDestination(player.position);
    }

    protected override void ResetValues()
    {
        isFirstFrameAfterDetectionExit = true;
        isPlayerOutOfDetection = false;
    }
}