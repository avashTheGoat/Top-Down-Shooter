using UnityEngine;

public class EnemyAttackChasing : MonoBehaviour, IAttack
{
    private Transform trans;

    private void Awake() => trans = transform;

    public float GetWeaponRotationChange(Transform _weapon)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            Vector2 _playerDirection = (_player.position - trans.position).normalized;
            Vector2 _weaponDirection = (_weapon.position - trans.position).normalized;

            float _deltaAngle = Vector2.SignedAngle(_weaponDirection, _playerDirection);

            return _deltaAngle;
        }

        return 0f;
    }

    public bool ShouldAttack(Weapon _weapon) => false;
}