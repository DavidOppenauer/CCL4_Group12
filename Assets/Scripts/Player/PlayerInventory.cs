using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> collectedItems = new HashSet<string>();

    public void AddItem(string itemId)
    {
        collectedItems.Add(itemId);
    }

    public bool HasItem(string itemId)
    {
        return collectedItems.Contains(itemId);
    }
}