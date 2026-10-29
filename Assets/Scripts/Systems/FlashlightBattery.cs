using UnityEngine;

public class FlashlightBattery : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    
    [Header("Battery")]
    [SerializeField] private float maxBatteryLife = 100f;
    [SerializeField] private float batteryLife = 50f;
    [SerializeField] private float batteryDrainRate = 5f;
    
    private void Update()
    {
     //   Debug.Log($"Battery Life: {batteryLife}");
        if (targetLight == null)
        {
           // Debug.LogWarning("Target light is not assigned.");
            return;
        }

        if (targetLight.enabled && batteryLife > 0f)
        {
            DrainBattery(batteryDrainRate * Time.deltaTime);
        }

        if(batteryLife <= 0f)
        {
            targetLight.enabled = false;
        }
    }

    public bool HasCharge()
    {
        return batteryLife > 0f;
    }

    public void RechargeBattery(float amount)
    {
        batteryLife += amount;
        batteryLife = Mathf.Clamp(batteryLife, 0f, maxBatteryLife);
    }

    public void DrainBattery(float amount)
    {
        batteryLife -= amount;
        batteryLife = Mathf.Clamp(batteryLife, 0f, maxBatteryLife);
    }

    public float GetBatteryLife()
    {
        return batteryLife;
    }
}
