using UnityEngine;

public class EnemyChaseIfNearbyEnemyChasingController : MonoBehaviour
{
    [SerializeField] private EnemyWavesSpawner enemyWaves;

    [SerializeField] private float maxDistanceToStartChase;

    private void Update()
    {
        foreach (Transform _enemy in enemyWaves.SpawnedEnemies)
        {
            EnemyStateMachine _stateMachine = _enemy.GetComponent<EnemyStateMachine>();

            if (_stateMachine.CurrentState == _stateMachine.IdleState)
                continue;

            foreach (Transform _comparingEnemy in enemyWaves.SpawnedEnemies)
            {
                if (_enemy == _comparingEnemy)
                    continue;

                EnemyStateMachine _comparingStateMachine = _comparingEnemy.GetComponent<EnemyStateMachine>();
                if (_comparingStateMachine.CurrentState != _comparingStateMachine.IdleState)
                    continue;

                if (Vector2.Distance(_enemy.position, _comparingEnemy.position) <= maxDistanceToStartChase)
                    _comparingStateMachine.TransitionToState(_comparingStateMachine.ChaseState);
            }
        }
    }
}