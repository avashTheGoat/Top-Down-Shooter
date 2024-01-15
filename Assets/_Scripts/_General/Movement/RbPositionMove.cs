using UnityEngine;

public class RbPositionMove : MonoBehaviour, IMovable
{
    private Rigidbody2D rb;
    private Transform trans;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = transform;
    }

    public void Move(Vector2 _movementVector)
    {
        rb.MovePosition(new Vector2(trans.position.x, trans.position.y) + _movementVector);
    }
}