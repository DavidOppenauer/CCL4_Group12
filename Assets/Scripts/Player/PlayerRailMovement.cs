using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerRailMovement : MonoBehaviour
{
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
        if ( currentRail != previousRail )
        {
            railLength = currentRail.GetSplineContainer().CalculateLength();
            previousRail = currentRail;
        }
        //Handling the movement flag... surely this works
        if(playerInput.GetMoveForwardIsPressed())
        {
            isWalking = true;
            distancePercentage += movementDirection * speed * Time.deltaTime / railLength;
        } 
        else
        {
            isWalking = false;
        }

        if (playerInput.GetTurnAroundWasPressedThisFrame())
        {
            movementDirection *= -1f;
        }
        // Junction Logic
        // Stopping/Getting stuck at the end at the end
        if (distancePercentage > 1f)
        {
            distancePercentage = 1f;
            endOfRailReached = true;
        }
        // This may be really important... to have no <= because the it would immeadiatly trigger when we first place the player on a rail
        if (distancePercentage < 0f)
        {
            distancePercentage = 0f;
            startOfRailReached = true;
        }

        currentPosition = currentRail.GetSplineContainer().EvaluatePosition(distancePercentage);
        Vector3 forward = currentRail.GetSplineContainer().EvaluateTangent(distancePercentage);

        if (movementDirection == -1f) {
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
}
