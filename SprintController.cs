// to sprint: push in left joystick

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class SprintController : MonoBehaviour
{
    [Header("Movement")]
    public float normalSpeed = 1f;
    public float sprintMultiplier = 1.5f; // can slow down or speed up

    [Header("Stamina")]
    public float maxSprintDuration = 8f; // can increase or reduce
    public float rechargeDuration = 12f; // can increase or reduce

    private float currentSprintTime;
    private bool isSprinting;
    private bool isRecharging;

    public DynamicMoveProvider moveProvider;

    private void Start()
    {
        currentSprintTime = maxSprintDuration;

        if (moveProvider != null)
        {
            moveProvider.moveSpeed = normalSpeed;
        }
    }

    private void Update()
    {
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
