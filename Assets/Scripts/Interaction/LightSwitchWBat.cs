using UnityEngine;

/// <summary>
/// Toggles an assigned Light when used.
/// </summary>
public sealed class LightSwitch : MonoBehaviour, IXRUsable
{
    [SerializeField] private Light targetLight;

    [SerializeField] private FlashlightBattery battery;

    public void Use()
    {
        if (targetLight == null)
        {
            Debug.LogWarning(
                "No target light has been assigned.", this);

            return;
        }

        if (targetLight.enabled)
        {
            targetLight.enabled = false;
            return;
        }

        if (battery != null && battery.HasCharge())
        {
            targetLight.enabled = true;
        }
        else
        {
            Debug.LogWarning(
                "Cannot turn on the light. No battery or insufficient charge.",
                this);
        }
    }
}