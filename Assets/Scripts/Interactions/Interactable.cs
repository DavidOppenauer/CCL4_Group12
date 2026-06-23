using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private string promptText = "Press E to interact";

    public string GetPromptText()
    {
        return promptText;
    }

    public abstract void Interact(PlayerInventory playerInventory, MessageBoxManager messageBoxManager);
}