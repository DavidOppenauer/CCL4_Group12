using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerRailMovement : MonoBehaviour
{
    [SerializeField] private LayerMask blockerLayer;
    [SerializeField] private float collisionRadius = 0.35f;
    [SerializeField] private float collisionHeightOffset = 1f;
    [SerializeField] private float speed = 1f;

    // New Input System
    [SerializeField] private PlayerInputs playerInput;

    // Flag for the animator
    private bool isWalking = false;

    private RailSegment previousRail;
    private bool endOfRailReached = false;
    private bool startOfRailReached = false;
    private float railLength;
    private float distancePercentage;
    private float movementDirection = 1f;
    private Vector3 currentPosition;

// Update is called once per frame
public void HandleMovement(RailSegment currentRail)
{
    // Only recalculate if its a new rail
    if (currentRail != previousRail)
    {
        railLength = currentRail.GetSplineContainer().CalculateLength();
        previousRail = currentRail;
    }

    if (playerInput.GetTurnAroundWasPressedThisFrame())
    {
        movementDirection *= -1f;
    }

    //Handling the movement flag... surely this works
    isWalking = false;

    if (playerInput.GetMoveForwardIsPressed())
    {
        // 1. Calculate where we WOULD move next
        // First we calculate where the player WOULD move next
        float nextDistancePercentage = distancePercentage;
        nextDistancePercentage += movementDirection * speed * Time.deltaTime / railLength;

        
        // 2. Clamp it and remember if we reached a junction
        // Junction Logic
        bool wouldReachEnd = false;
        bool wouldReachStart = false;

        // Stopping/Getting stuck at the end at the end
        if (nextDistancePercentage > 1f)
        {
            nextDistancePercentage = 1f;
            wouldReachEnd = true;
        }

        // This may be really important... to have no <= because the it would immeadiatly trigger when we first place the player on a rail
        if (nextDistancePercentage < 0f)
        {
            nextDistancePercentage = 0f;
            wouldReachStart = true;
        }

        // 3. Get the target position from the spline
        Vector3 nextPosition = currentRail.GetSplineContainer().EvaluatePosition(nextDistancePercentage);

        // 4. Check if something blocks the way
        // Only move if the next position is not blocked by a solid door/blocker
        if (!MovementIsBlocked(nextPosition))
        {
            // 5. Only now actually accept the movement
            distancePercentage = nextDistancePercentage;
            endOfRailReached = wouldReachEnd;
            startOfRailReached = wouldReachStart;
            isWalking = true;
        }
    }  

    // Always update position/rotation based on the accepted distancePercentage
    currentPosition = currentRail.GetSplineContainer().EvaluatePosition(distancePercentage);
    Vector3 forward = currentRail.GetSplineContainer().EvaluateTangent(distancePercentage);

    if (movementDirection == -1f) 
    {
        forward = -forward;
    }

    transform.position = currentPosition;
    transform.forward = forward;
}

    public bool GetEndOfRailReached()
    {
        return endOfRailReached;
    }

    public bool GetStartOfRailReached()
    {
        return startOfRailReached;
    }

    public void ResetJunctionFlags()
    {
        startOfRailReached = false;
        endOfRailReached = false;
    }

    public void PlaceOnRailStart()
    {
        distancePercentage = 0f;
        movementDirection = 1f;
    }
    public void PlaceOnRailEnd()
    {
        distancePercentage = 1f;
        movementDirection = -1f;
    }

    public bool GetIsPlayerWalking()
    {
        return isWalking;
    }

    public void SetIsPlayerWalking(bool state)
    {
        isWalking = state;
    }

    private bool MovementIsBlocked(Vector3 targetPosition)
    {
        Vector3 currentCheckPosition = transform.position + Vector3.up * collisionHeightOffset;
        Vector3 targetCheckPosition = targetPosition + Vector3.up * collisionHeightOffset;

        Vector3 moveDirection = targetCheckPosition - currentCheckPosition;
        float moveDistance = moveDirection.magnitude;

        if (moveDistance <= 0f)
        {
            return false;
        }

        if (Physics.SphereCast(
            currentCheckPosition,
            collisionRadius,
            moveDirection.normalized,
            out RaycastHit hit,
            moveDistance,
            blockerLayer,
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Movement blocked by: " + hit.collider.name);
            return true;
        }

        return false;
    }
}
