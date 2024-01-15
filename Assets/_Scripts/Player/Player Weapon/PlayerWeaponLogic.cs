using UnityEngine;

public class PlayerWeaponLogic : MonoBehaviour, IAttack, IReload
{
    [SerializeField] private KeyCode attackKey;
    [SerializeField] private KeyCode reloadKey;

    private Transform trans;

    private void Awake()
    {
        trans = transform;
    }

    public float GetWeaponRotationChange(Transform _weapon)
    {
        Vector2 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float _deltaAngle = Vector2.SignedAngle((_weapon.position - trans.position).normalized, (_mousePosition - (Vector2)trans.position).normalized);

        return _deltaAngle;
    }

    public bool ShouldAttack(Weapon _weapon) => Input.GetKey(attackKey);

    public bool ShouldReload(RangedWeapon _weapon) => Input.GetKey(reloadKey);
}