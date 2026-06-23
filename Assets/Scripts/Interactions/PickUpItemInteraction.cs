using UnityEngine;

public class PickUpItemInteraction : Interactable
{
    [Header("Item")]
    [SerializeField] private string itemId = "red_key";

    [TextArea]
    [SerializeField] private string pickupMessage = "You picked up a key.";

    public override void Interact(PlayerInventory playerInventory, MessageBoxManager messageBoxManager)
    {
        playerInventory.AddItem(itemId);

        if (messageBoxManager != null)
        {
            messageBoxManager.ShowMessage(pickupMessage);
        }

        gameObject.SetActive(false);
    }
}