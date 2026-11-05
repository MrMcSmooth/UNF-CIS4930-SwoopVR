using UnityEngine;

public class debugRay : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float rayDistance = 20f;

    void Update()
    {
        if (targetLight == null)
        {
            Debug.LogWarning("FlashRayCast: Target Light is missing!");
            return;
        }

        if (rayOrigin == null)
        {
            Debug.LogWarning("FlashRayCast: Ray Origin is missing!");
            return;
        }

        if (!targetLight.enabled)
        {
            return;
        }

        Debug.DrawRay(
            rayOrigin.position,
            rayOrigin.forward * rayDistance,
            Color.red
        );

        RaycastHit hit;

        if (Physics.Raycast(
            rayOrigin.position,
            rayOrigin.forward,
            out hit,
            rayDistance))
        {
           // Debug.Log("Flashlight hit: " + hit.collider.gameObject.name);

            AlienLogic alien =
                hit.collider.GetComponentInParent<AlienLogic>();

            if (alien != null)
            {
                Debug.Log("ALIEN FOUND - SCARING ALIEN");

                alien.Scared();
            }
        }
    }
}