using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text objectiveText;

    private Coroutine currentDialogue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetObjective(string objective)
    {
        objectiveText.text = "OBJECTIVE:\n" + objective;
    }

    public void ShowDialogueThenObjective(
        string dialogue,
        float duration,
        string objective)
    {
        if (currentDialogue != null)
        {
            StopCoroutine(currentDialogue);
        }

        currentDialogue =
            StartCoroutine(DialogueThenObjectiveRoutine(
                dialogue,
                duration,
                objective));
    }

    private IEnumerator DialogueThenObjectiveRoutine(
        string dialogue,
        float duration,
        string objective)
    {
        dialogueText.text = dialogue;

        yield return new WaitForSeconds(duration);

        dialogueText.text = "";

        SetObjective(objective);
    }

    public void ClearObjective()
    {
        objectiveText.text = "";
    }
}
