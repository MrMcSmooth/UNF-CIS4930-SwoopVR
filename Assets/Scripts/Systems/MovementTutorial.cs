using UnityEngine;

public class MovementTutorial : MonoBehaviour
{
    public CharacterController playerController;

    private Vector3 startPosition;
    private bool hasMoved = false;

    void Start()
    {
        startPosition = playerController.transform.position;
    }

    void Update()
    {
        if (hasMoved)
            return;

        float distanceMoved =
            Vector3.Distance(
                startPosition,
                playerController.transform.position);

        if (distanceMoved > 0.5f)
        {
            hasMoved = true;

            DialogueManager.Instance.ShowDialogueThenObjective(
                "A bit dark in here, innit?",
                5f,
                "Find the flashlight."
            );
        }
    }
}
