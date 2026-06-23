using UnityEngine;

public class LockedDoorInteraction : MonoBehaviour
{
    private bool playerHasKey = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
        }
    }
}
