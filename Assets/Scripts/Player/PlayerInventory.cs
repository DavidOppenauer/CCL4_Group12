using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private bool hasKey_1 = false;
    private bool hasKey_2 = false;    
    private bool hasKey_3 = false;

    private void SetHasKey_1(bool value)
    {
        hasKey_1 = value;
    }
    private void SetHasKey_2(bool value)
    {
        hasKey_2 = value;
    }
    private void SetHasKey_3(bool value)
    {
        hasKey_3 = value;
    }
}