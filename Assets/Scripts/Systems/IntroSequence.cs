using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.ShowDialogueThenObjective(
            "Up and at em.",
            5f,
            "Use the joystick to move around."
        );
    }
}
