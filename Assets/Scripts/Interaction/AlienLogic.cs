using UnityEngine;

public class AlienLogic : MonoBehaviour
{
    [SerializeField] private Material newMaterial;
    [SerializeField] private float sMaxDuration = 100f;
    [SerializeField] public float sDuration = 100f;
    [SerializeField] private float sDrain = 5f;

    private bool isScared = false;

    void Update()
    {
        if (isScared)
        {
            DrainAlien(sDrain * Time.deltaTime);
            Debug.Log(sDuration);
        }

        if(sDuration <= 0)
        {
            isScared = false;
        }
    }

    public void Scared()
    {
        if (isScared)
        {
            return;
        }
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = newMaterial;
        isScared = true;
    }

    public void DrainAlien(float amount)
    {
        sDuration -= amount;
        sDuration = Mathf.Clamp(sDuration, 0f, sMaxDuration);
    }
}
