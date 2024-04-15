using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour, IMover
{
    public static event Action OnPlayerMove;

    [HideInInspector]
    public bool CanSprint;

    public float WalkSpeed => walkSpeed;
    public float SprintSpeedMultiplier => sprintSpeedMultiplier;

    [Min(0f)]
    [SerializeField] private float walkSpeed;
    [Min(0f)]
    [SerializeField] private float sprintSpeedMultiplier;

    private IMoveable movementMethod;
    private Vector2 movementVector;
    private float speed;

    private float originalSpeed;

    private void Awake()
    {
        movementMethod = GetComponent<IMoveable>();

        if (movementMethod == null)
            Debug.LogError("The player does not have an IMovable attached to it");

        CanSprint = true;

        originalSpeed = walkSpeed;
    }

    private void Update()
    {
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        speed = (Input.GetKey(KeyCode.LeftShift) && CanSprint) ? (sprintSpeedMultiplier * walkSpeed) : walkSpeed;
    }

    private void FixedUpdate()
    {
        movementVector.Normalize();
        movementVector *= speed * Time.fixedDeltaTime;

        movementMethod.Move(movementVector);

        if (movementVector.magnitude > 0f)
            OnPlayerMove?.Invoke();
    }

    public float GetOriginalMovementSpeed() => originalSpeed;
    public float GetMovementSpeed() => speed;

    public void SetMovementSpeed(float _newSpeed)
    {
        if (_newSpeed < 0f)
            throw new ArgumentException($"{nameof(_newSpeed)} cannot be negative. It is {_newSpeed}.");

        walkSpeed = _newSpeed;
    }
}