using System;
using UnityEngine;

public class RailJunction : MonoBehaviour
{
    [SerializeField] private RailSegment[] railSegments;

    [Serializable]
    public class DirectionSetup
    {
        public RailSegment incomingRail;
        public RailSegment leftRail;
        public RailSegment straightRail;
        public RailSegment rightRail;
    }

    [SerializeField] private DirectionSetup[] directionSetups;

    public RailSegment[] GetRailSegments()
    {
        return railSegments;
    }

    public int GetRailCount()
    {
        return railSegments.Length;
    }

    public JunctionManager.JunctionConfig GetJunctionConfig(RailSegment incomingRail)
    {
        DirectionSetup setup = GetSetupForIncomingRail(incomingRail);

        JunctionManager.JunctionConfig config = new JunctionManager.JunctionConfig();

        if (setup == null)
        {
            return config;
        }

        config.allowLeft = setup.leftRail != null;
        config.allowStraight = setup.straightRail != null;
        config.allowRight = setup.rightRail != null;

        return config;
    }

    public RailSegment GetRailForDirection(RailSegment incomingRail, string direction)
    {
        DirectionSetup setup = GetSetupForIncomingRail(incomingRail);

        if (setup == null)
        {
            Debug.LogWarning("No direction setup found for incoming rail: " + incomingRail.name);
            return null;
        }

        if (direction == "Left")
        {
            return setup.leftRail;
        }

        if (direction == "Straight")
        {
            return setup.straightRail;
        }

        if (direction == "Right")
        {
            return setup.rightRail;
        }

        Debug.LogWarning("Unknown direction: " + direction);
        return null;
    }

    private DirectionSetup GetSetupForIncomingRail(RailSegment incomingRail)
    {
        foreach (DirectionSetup setup in directionSetups)
        {
            if (setup.incomingRail == incomingRail)
            {
                return setup;
            }
        }

        return null;
    }
}