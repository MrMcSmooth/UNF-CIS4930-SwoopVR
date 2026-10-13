using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Forwards an XR Interactable's Activated event to every attached
/// component that implements IXRUsable.
/// </summary>
[RequireComponent(typeof(XRBaseInteractable))]
public sealed class XRUseDispatcher : MonoBehaviour
{
    private XRBaseInteractable interactable;
    private MonoBehaviour[] attachedBehaviours;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        attachedBehaviours = GetComponents<MonoBehaviour>();
    }

    private void OnEnable()
    {
        interactable.activated.AddListener(OnActivated);
    }

    private void OnDisable()
    {
        interactable.activated.RemoveListener(OnActivated);
    }

    private void OnActivated(ActivateEventArgs eventArgs)
    {
        foreach (MonoBehaviour behaviour in attachedBehaviours)
        {
            if (behaviour is IXRUsable usable)
            {
                usable.Use();
            }
        }
    }
}