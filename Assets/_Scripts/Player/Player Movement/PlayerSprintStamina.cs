using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerSprintStamina : MonoBehaviour
{
    [field: SerializeField] public float MaxStamina { get; private set; }
    public float Stamina { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] private float staminaIncreasePercentPerSecond;
    [Range(0f, 1f)]
    [SerializeField] private float staminaDecreasePercentPerSecond;
    [SerializeField] private float staminaIncreaseDelay;

    private PlayerMovement playerMovement;
    private bool hasRunOutOfStaminaWhileSprinting;
    private float staminaIncreaseDelayTimer = 0f;

    private void Awake()
    {
        Stamina = MaxStamina;
        playerMovement = GetComponent<PlayerMovement>();
        hasRunOutOfStaminaWhileSprinting = false;
    }

    private void Update()
    {
        //hasRunOutOfStaminaWhileSprinting is to prevent player from sprinting (and losing stamina) if they have ran out of stamina
        //while holding down shift until either they reached max stamina (hasRunOutOfStaminaWhileSprinting = stamina != maxStamina)
        //or the player stopped pressing shift

        if (Input.GetKey(KeyCode.LeftShift))
        {
            staminaIncreaseDelayTimer = staminaIncreaseDelay;

            if (!hasRunOutOfStaminaWhileSprinting)
            {
                Stamina -= MaxStamina * staminaDecreasePercentPerSecond * Time.deltaTime;

                if (Stamina <= 0f)
                    hasRunOutOfStaminaWhileSprinting = true;
            }
        }

        if (!Input.GetKey(KeyCode.LeftShift) || hasRunOutOfStaminaWhileSprinting)
        {
            if (staminaIncreaseDelayTimer == 0f)
            {
                Stamina += MaxStamina * staminaIncreasePercentPerSecond * Time.deltaTime;

                if (hasRunOutOfStaminaWhileSprinting)
                    hasRunOutOfStaminaWhileSprinting = Stamina != MaxStamina;

                if (!Input.GetKey(KeyCode.LeftShift))
                    hasRunOutOfStaminaWhileSprinting = false;
            }

            else
            {
                staminaIncreaseDelayTimer -= Time.deltaTime;
                staminaIncreaseDelayTimer = Mathf.Clamp(staminaIncreaseDelayTimer, 0f, staminaIncreaseDelay);
            }
        }

        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);
        playerMovement.CanSprint = Stamina > 0f && !hasRunOutOfStaminaWhileSprinting;
    }
}