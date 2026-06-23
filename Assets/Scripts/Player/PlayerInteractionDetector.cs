using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    private PickUpKeyInteraction currentPickupKeyInteraction;
    private LockedDoorInteraction currentLockedDoorInteraction;

    private bool playerHasEnteredInteraction = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LockedDoorInteraction")
        {
            playerHasEnteredInteraction = true;
            currentLockedDoorInteraction = other.gameObject.GetComponent<LockedDoorInteraction>();
        }
        if (other.gameObject.tag == "PickUpKeyInteraction")
        {
            playerHasEnteredInteraction = true;
            currentPickupKeyInteraction = other.gameObject.GetComponent<PickUpKeyInteraction>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LockedDoorInteraction" || other.gameObject.tag == "PickUpKeyInteraction")
        {
            playerHasEnteredInteraction = false;
            currentLockedDoorInteraction = null;
            currentPickupKeyInteraction = null;
        }
    }

    public bool GetPlayerHasEnteredInteraction()
    {
        return playerHasEnteredInteraction;
    }

}
