using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    [Header("Important Things")]
    [SerializeField] private Transform camera;
    [SerializeField] private Transform startCheckpoint;

    private CharacterController cController;

    private Vector3 checkPos;
    private Quaternion checkRot;

    private void Awake()
    {
        cController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (startCheckpoint != null)
        {
            checkPos = startCheckpoint.position;
            checkRot = startCheckpoint.rotation;
        }
        else
        {
            checkPos = transform.position;
            checkRot = transform.rotation;

            if(camera != null)
            {
                checkPos.x = camera.position.x;
                checkPos.z = camera.position.z;
            }
       
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if(checkpoint == null) return;

        checkPos = checkpoint.position;
        checkRot = checkpoint.rotation;
        //Debug.Log("Checkpoint set to: " + checkPos + ", " + checkRot);
    }

    public void Die()
    {
        Debug.Log("Player died! Respawning...");
        Respawn();
    }

    public void Respawn()
    {
        if (camera == null)
        {
            Debug.LogError("Camera not assigned!");
            return;
        }

        bool hasCharacterController = cController != null && cController.enabled;

        if(hasCharacterController) cController.enabled = false; 
        
        transform.rotation = checkRot;

        Vector3 headOffset = camera.position - transform.position;
        headOffset.y = 0f;

        transform.position = checkPos - headOffset;

        if(hasCharacterController) cController.enabled = true;

        Debug.Log("Player respawned at: " + checkPos + ", " + checkRot);
    } 
}
