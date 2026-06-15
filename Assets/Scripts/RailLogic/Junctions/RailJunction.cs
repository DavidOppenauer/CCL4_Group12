using UnityEngine;

public class RailJunction : MonoBehaviour
{
    [SerializeField] private RailSegment[] connectedRails;

    public RailSegment[] GetRailSegments()
    {
        return connectedRails;
    }

    public int GetRailCount()
    {
        return connectedRails.Length;
    }
}
