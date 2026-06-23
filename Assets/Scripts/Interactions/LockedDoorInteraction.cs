using UnityEngine;

public class LockedDoorInteraction : Interactable
{
    [Header("Required Item")]
    [SerializeField] private string requiredItemId = "red_key";

    [Header("Door / Blocker")]
    [SerializeField] private GameObject doorBlocker;

    [TextArea]
    [SerializeField] private string lockedMessage = "The door is locked. You need a key.";

    [TextArea]
    [SerializeField] private string unlockedMessage = "You unlocked the door.";

    public override void Interact(PlayerInventory playerInventory, MessageBoxManager messageBoxManager)
    {
        if (playerInventory.HasItem(requiredItemId))
        {
            if (messageBoxManager != null)
            {
                messageBoxManager.ShowMessage(unlockedMessage);
            }

            if (doorBlocker != null)
            {
                doorBlocker.SetActive(false);
            }
            gameObject.SetActive(false);
        }
        else
        {
            if (messageBoxManager != null)
            {
                messageBoxManager.ShowMessage(lockedMessage);
            }
        }
    }
}