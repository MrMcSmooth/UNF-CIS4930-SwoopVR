
using UnityEngine;

public class FlashRayCast : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private float rayDistance = 10f;

    void Update()
    {
        if (targetLight == null || !targetLight.isActiveAndEnabled)
        {
            return;
        }

        Ray ray = new Ray(
            targetLight.transform.position,
            targetLight.transform.forward
        );

        Debug.DrawRay(
            ray.origin,
            ray.direction * rayDistance,
            Color.red
        );

        RaycastHit hitThatThang;

        if (Physics.Raycast(
            ray,
            out hitThatThang,
            rayDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide))
        {
            AlienLogic alien =
                hitThatThang.collider.GetComponentInParent<AlienLogic>();

            Debug.Log("Raycast hit: " + hitThatThang.collider.name);

            if (alien != null)
            {
                Debug.Log("Alien detected! Scaring...");
                alien.Scared();
            }
        }
    }
}
