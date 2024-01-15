using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public static event Action OnPlayerMove;
    [HideInInspector]
    public bool CanSprint;

    [Min(0f)]
    [SerializeField] private float walkSpeed;
    [Min(0f)]
    [SerializeField] private float sprintSpeed;

    private IMovable movementMethod;
    private Vector2 movementVector;
    private float speed;

    private void Awake()
    {
        movementMethod = GetComponent<IMovable>();

        if (movementMethod == null)
        {
            Debug.LogError("The player does not have an IMovable attached to it");
        }

        CanSprint = true;
    }

    private void Update()
    {
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        speed = Input.GetKey(KeyCode.LeftShift) && CanSprint ? sprintSpeed : walkSpeed;
    }

    private void FixedUpdate()
    {
        movementVector.Normalize();
        movementVector *= speed * Time.fixedDeltaTime;

        movementMethod.Move(movementVector);

        if (movementVector.magnitude > 0f)
        {
            OnPlayerMove?.Invoke();
        }
    }
}