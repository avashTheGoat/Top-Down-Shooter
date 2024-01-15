using UnityEngine;

public class EnemyAttackAttacking : MonoBehaviour, IAttack
{
    private Transform trans;

    private void Awake() => trans = GetComponent<Transform>();

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

    public bool ShouldAttack(Weapon _weapon)
    {
        if (_weapon is RangedWeapon)
        {
            RangedWeapon _rangedWeapon = _weapon as RangedWeapon;

            if (PlayerProvider.TryGetPlayer(out Transform _player))
            {
                return Vector2.Distance(_player.position, _rangedWeapon.transform.position) <= _rangedWeapon.Range;
            }
        }

        else if (_weapon is MeleeWeapon)
        {
            MeleeWeapon _meleeWeapon = _weapon as MeleeWeapon;

            if (PlayerProvider.TryGetPlayer(out Transform _player))
            {
                return _meleeWeapon.GetGameObjectsInAttackAOE().Contains(_player.gameObject);
            }
        }

        else Debug.LogError("An unidentified weapon has been detected.");

        return false;
    }
}