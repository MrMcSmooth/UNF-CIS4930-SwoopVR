using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    [Header("Battery UI Settings")]
    [SerializeField] private FlashlightBattery battery;
    [SerializeField] private Image greenFill;    

    void Update()
    {
        if(battery == null || greenFill == null)
        {
            return;
        }

        float currBattery = battery.GetBatteryLife();
        float maxBattery = battery.GetMaxBatteryLife();

        if (maxBattery <= 0f)
        {
            greenFill.fillAmount = 0f;
            return;
        }

        float batteryPercentage = currBattery / maxBattery;

        greenFill.fillAmount = Mathf.Clamp01(batteryPercentage);
    }
}
