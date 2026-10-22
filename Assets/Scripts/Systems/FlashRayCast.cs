using UnityEngine;
using static RespawnTrigger;

public class FlashRayCast : MonoBehaviour
{

    [SerializeField] private Light targetLight;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitThatThang;

        //AlienLogic aL = new ColliderHit().transform.GetComponent<AlienLogic>;

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (Physics.Raycast(ray, out hitThatThang) && targetLight.enabled)
        {
            GameObject Target = hitThatThang.transform.gameObject;

            if(Target.name == "Alien")
            {
                AlienLogic AL = Target.GetComponent<AlienLogic>();
                AL.Scared();
            }

            Debug.Log($"Hit {hitThatThang.collider.gameObject.name} at distance {hitThatThang.distance}");
        }


    }
}
