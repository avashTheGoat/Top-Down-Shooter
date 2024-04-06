using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ForceMove : MonoBehaviour, IMoveable
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 _movementVector)
    {
        rb.AddForce(_movementVector);
    }
}