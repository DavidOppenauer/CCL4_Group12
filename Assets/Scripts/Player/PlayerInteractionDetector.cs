using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    [SerializeField] private MessageBoxManager messageBoxManager;
    private Interactable currentInteractable;
    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponentInParent<Interactable>();

        if (interactable != null)
        {
            currentInteractable = interactable;

            if (messageBoxManager != null)
            {
                messageBoxManager.ShowPrompt(interactable.GetPromptText());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponentInParent<Interactable>();

        if (interactable != null && interactable == currentInteractable)
        {
            ClearCurrentInteraction();
        }
    }

    public bool HasInteraction()
    {
        return currentInteractable != null && currentInteractable.gameObject.activeInHierarchy;
    }

    public Interactable GetCurrentInteractable()
    {
        if (!HasInteraction())
        {
            return null;
        }

        return currentInteractable;
    }

    public void ClearCurrentInteraction()
    {
        currentInteractable = null;

        if (messageBoxManager != null)
        {
            messageBoxManager.HidePrompt();
        }
    }
}