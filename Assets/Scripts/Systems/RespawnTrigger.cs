using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Death,
        Checkpoint
    }

    [SerializeField] private TriggerType triggerType;
    [SerializeField] private Transform checkpointLocation;

    private bool firstTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCheckpoint playerCheckpoint = other.GetComponentInParent<PlayerCheckpoint>();

        if(playerCheckpoint == null)
        {
            return;
        }

        if(triggerType == TriggerType.Death)
        {
            playerCheckpoint.Die();
        }
        else if (triggerType == TriggerType.Checkpoint && !firstTrigger)
        {
            if(checkpointLocation != null)
            {
                playerCheckpoint.SetCheckpoint(checkpointLocation);
                firstTrigger = true;
            } else
            {
                Debug.LogWarning("Checkpoint location is not set for the checkpoint trigger.");
            }
        }
    }
}
