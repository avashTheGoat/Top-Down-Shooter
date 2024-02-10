using UnityEngine;

public class EnemyAttackAttacking : MonoBehaviour, IAttack
{
    private Transform trans;

    private void Awake() => trans = transform;

    public float GetWeaponRotationChange(Transform _weapon)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            Vector2 _playerDirection = _player.position - trans.position;
            Vector2 _weaponDirection = _weapon.position - trans.position;

            print("Player direction: " + _playerDirection);

            float _deltaAngle = Vector2.SignedAngle(_weaponDirection.normalized, _playerDirection.normalized);

            return _deltaAngle;
        }

        return 0f;
    }

    public bool ShouldAttack(Weapon _weapon) => true;
}