using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CrankCharger : MonoBehaviour
{
    [Header("Stuff")]
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private FlashlightBattery battery;

    [Header("Variables")]
    [SerializeField] private float chargeRate = 20f;
    [SerializeField] private float minMove = 0.1f;

    private float lastAngle;

    private void Awake()
    {
        if(hinge == null) hinge = GetComponent<HingeJoint>();

        if(grabInteractable == null) grabInteractable = GetComponent<XRGrabInteractable>();

    }

    private void Start()
    {
        if(hinge != null) lastAngle = hinge.angle;
    }

    private void FixedUpdate()
    {
        if (hinge == null || battery == null) return;

        if(grabInteractable == null || !grabInteractable.isSelected)
        {
            lastAngle = hinge.angle;
            return;
        }

        float currentAngle = hinge.angle;

        float angleDelta = Mathf.Abs(Mathf.DeltaAngle(currentAngle, lastAngle));

        lastAngle = currentAngle;


        if (angleDelta < minMove) return;

        float chargeAmount = (angleDelta / 360f) * chargeRate;

        battery.RechargeBattery(chargeAmount);
    }
}
