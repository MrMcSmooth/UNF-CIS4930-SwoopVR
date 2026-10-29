// to sprint: push in left joystick

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class SprintController : MonoBehaviour
{
    [Header("Movement")]
    public float normalSpeed = 1f;
    public float sprintMultiplier = 8f;

    [Header("Stamina")]
    public float maxSprintDuration = 8f;
    public float rechargeDuration = 12f;

    [Header("Input")]
    public InputActionReference sprintButton;

    private float currentSprintTime;
    private bool isSprinting;
    private bool isRecharging;

    public ContinuousMoveProvider moveProvider; // DynamicMoveProvider was deprecated, using ContinuousMoveProvider instead

    private void OnEnable()
    {
        if (sprintButton != null && sprintButton.action != null)
        {
            sprintButton.action.Enable();
            sprintButton.action.performed += OnSprintPerformed;
        }
    }

    private void OnDisable()
    {
        if (sprintButton != null && sprintButton.action != null)
        {
            sprintButton.action.performed -= OnSprintPerformed;
        }
    }

    private void Start()
    {
        currentSprintTime = maxSprintDuration;

        if (moveProvider == null)
        {
            moveProvider = GetComponentInChildren<ContinuousMoveProvider>();
        }

        if (moveProvider != null)
        {
            moveProvider.moveSpeed = normalSpeed;
        }
    }

    private void Update()
    {
        // check for left shift on keyboard
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            Debug.Log("Shift was pressed.");
            ToggleSprint();
        }

        // stamina drain / recharge logic
        if (isSprinting)
        {
            currentSprintTime -= Time.deltaTime;

            if (currentSprintTime <= 0)
            {
                StopSprint();
                StartRecharge();
            }
        }
        else if (isRecharging)
        {
            currentSprintTime += (maxSprintDuration / rechargeDuration) * Time.deltaTime;

            if (currentSprintTime >= maxSprintDuration)
            {
                currentSprintTime = maxSprintDuration;
                isRecharging = false;
            }
        }
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        ToggleSprint();
    }

    public void ToggleSprint()
    {
        if (isRecharging)
            return;

        if (isSprinting)
        {
            StopSprint();
        }
        else if (currentSprintTime > 0)
        {
            StartSprint();
        }
    }

    private void StartSprint()
    {
        isSprinting = true;

        if (moveProvider != null)
        {
            moveProvider.moveSpeed = normalSpeed * sprintMultiplier;

            // print speed right after setting it to verify
            Debug.Log("MoveProvider speed is now set to: " + moveProvider.moveSpeed);
        }
    }

    private void StopSprint()
    {
        isSprinting = false;

        if (moveProvider != null)
        {
            moveProvider.moveSpeed = normalSpeed;
        }
    }

    private void StartRecharge()
    {
        isRecharging = true;
    }

    public float GetStaminaPercent()
    {
        return currentSprintTime / maxSprintDuration;
    }
}