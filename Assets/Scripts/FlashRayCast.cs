using UnityEngine;

public class FlashRayCast : MonoBehaviour
{

    [SerializeField] private Light targetLight;
    [SerializeField] private float rayDistance = 10f;

    void Update()
    {
        if(targetLight == null || !targetLight.enabled)
        {
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        RaycastHit hitThatThang;

        //AlienLogic aL = new ColliderHit().transform.GetComponent<AlienLogic>;



        if (Physics.Raycast(ray, out hitThatThang) && targetLight.enabled)
        {
            AlienLogic alien = hitThatThang.collider.GetComponent<AlienLogic>();

            if (alien != null)
            {
                alien.Scared();
            }

            //Debug.Log($"Hit {hitThatThang.collider.gameObject.name} at distance {hitThatThang.distance}");
        }


    }
}
