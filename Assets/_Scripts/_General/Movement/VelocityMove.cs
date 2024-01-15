using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityMove : MonoBehaviour, IMovable
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 _movementVector)
    {
        if (_movementVector == Vector2.zero)
        {
            return;
        }

        rb.velocity = Vector2.Lerp(rb.velocity, _movementVector, 0.5f);
    }
}