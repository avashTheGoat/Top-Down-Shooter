using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float Damage { get; private set; }
    public float Range { get; private set; }

    private bool hasBeenInitialized = false;

    public void Init(float _damage, float _projectileSpeed, Vector2 _startPosition, float _zRotation, float _range)
    {
        if (!hasBeenInitialized)
        {
            Transform _transform = GetComponent<Transform>();

            _transform.position = _startPosition;
            _transform.Rotate(new Vector3(0, 0, _zRotation));
            Damage = _damage;
            GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0f, 0f, _zRotation) * new Vector2(1, 0) * _projectileSpeed;
            Range = _range;

            hasBeenInitialized = true;
        }

        else
        {
            Debug.LogError("Bullet cannot be initalized twice.");
        }
    }
}