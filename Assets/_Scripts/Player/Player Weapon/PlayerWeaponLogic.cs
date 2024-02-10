using UnityEngine;

public class PlayerWeaponLogic : MonoBehaviour, IAttack, IReload
{
    [SerializeField] private KeyCode attackKey;
    [SerializeField] private KeyCode reloadKey;

    private Transform trans;

    private void Awake() => trans = transform;

    public float GetWeaponRotationChange(Transform _weapon)
    {
        Vector2 _weaponDirection = _weapon.position - trans.position;
        Vector2 _mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - trans.position;

        float _deltaAngle = Vector2.SignedAngle(_weaponDirection.normalized, _mouseDirection.normalized);

        return _deltaAngle;
    }

    public bool ShouldAttack(Weapon _weapon) => Input.GetKey(attackKey);

    public bool ShouldReload(RangedWeapon _weapon) => Input.GetKey(reloadKey);
}