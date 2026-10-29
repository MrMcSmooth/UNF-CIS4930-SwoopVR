using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI tutorialText;

    [Header("Target Object")]
    public GameObject flashlight;

    public void OnFlashlightPickedUp()
    {
        // hide flashlight
        if (flashlight != null)
        {
            flashlight.SetActive(false);
        }

        // update text
        if (tutorialText != null)
        {
            tutorialText.text = "Nice work.";
        }
    }
}