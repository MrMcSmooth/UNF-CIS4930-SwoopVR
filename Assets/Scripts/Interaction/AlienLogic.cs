using UnityEngine;

public class AlienLogic : MonoBehaviour
{
    [SerializeField] private Material newMaterial;

    void Update()
    {
        
    }

    public void Scared()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial;
    }
}
