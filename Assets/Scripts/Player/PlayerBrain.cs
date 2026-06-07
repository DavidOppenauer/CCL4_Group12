using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] private RailSegment initialRail;
    private RailSegment currentRail;
    private RailJunction currentJunction;
    private PlayerRailMovement playerRailMovement;
    private RailSegment[] connectedRailsToCurrentJunction;
    private bool playerHasDecidedOnRoute = false;
    private void Awake()
    {
        playerRailMovement = GetComponent<PlayerRailMovement>();
        currentState = PlayerState.MovingAlongRail;
        currentRail = initialRail;
    }
    private enum PlayerState
    {
        MovingAlongRail,
        ChoosingNextRail,
        FirstPersonAiming
    }
    private PlayerState currentState;
    private PlayerState previousState;
    private enum JunctionType
    {
        WaitingForNewValue,
        EndJunction,
        StartJunction
    }

    private JunctionType currentJunctionType;

    private void Update()
    {
        Debug.Log(currentState);
        HandleState();
        switch (currentState)
        {
            case PlayerState.MovingAlongRail:
                // Call Movement script to allow player movement
                playerRailMovement.HandleMovement(currentRail);
                // "Listen" for when the end of a rail was reached specifically the beginning or the end of the rail
                if (playerRailMovement.GetEndOfRailReached())
                {
                    // Little saftey guard, check if there actually is a junction assigned in the inspector
                    if (currentRail.GetEndJunction() != null)
                    {
                        // Set the current Junction
                        currentJunction = currentRail.GetEndJunction();
                        // Tell the movement script that it was heard and reset the bools that get set when you reach the end of a rail
                        playerRailMovement.ResetJunctionFlags();
                        // Stop early if theres just the one rail its connected to, its a dead end
                        if (currentJunction.GetRailCount() == 1)
                        {
                            return;
                        }
                        // Clarify if its the start or end for the next state
                        currentJunctionType = JunctionType.EndJunction;
                        // Set the variable I use for only running code once right after switch of state
                        previousState = currentState;
                        // Proceed to next state
                        currentState = PlayerState.ChoosingNextRail;
                    } 
                    else
                    {
                        return;
                    }
                }
                if (playerRailMovement.GetStartOfRailReached())
                {
                    if (currentRail.GetStartJunction() != null)
                    {
                        currentJunction = currentRail.GetStartJunction();
                        playerRailMovement.ResetJunctionFlags();
                        if (currentJunction.GetRailCount() == 1)
                        {
                            return;
                        }
                        currentJunctionType = JunctionType.StartJunction;
                        previousState = currentState;
                        currentState = PlayerState.ChoosingNextRail;
                    } 
                    else
                    {
                        return;
                    }
                }
            break;
            case PlayerState.ChoosingNextRail:
            
                // Getting the variable only in the first iteration
                if (currentState != previousState)
                {
                    connectedRailsToCurrentJunction = currentJunction.GetRailSegments();
                    previousState = currentState;                  
                }

                // ---------------------------------------------------------------------------------
                // THIS NEXT PART IS REALLY EXPERIMENTAL AND NEEDS GOOD CARE AND THOUGHT LATER ON...
                // ---------------------------------------------------------------------------------

                // Setting currentRail to what the player chooses
                // 0 should be the currentRail 1 and 2 should be different... Oh god I have to be careful in the inspector setting up references
                if(Input.GetKeyDown(KeyCode.A))// Left for now
                {
                    currentRail = connectedRailsToCurrentJunction[1];
                    playerHasDecidedOnRoute = true;
                }
                if(Input.GetKeyDown(KeyCode.D))// Left for now
                {
                    currentRail = connectedRailsToCurrentJunction[2];
                    playerHasDecidedOnRoute = true;
                }
                if (playerHasDecidedOnRoute == true)
                {
                    // Put the player at the corresponding Position meaning at the start or end of the rail
                    if(currentJunctionType == JunctionType.EndJunction)
                    {
                        playerRailMovement.PlaceOnRailStart();
                    }
                    if (currentJunctionType == JunctionType.StartJunction)
                    {
                        playerRailMovement.PlaceOnRailEnd();
                    }
                    // After everythings done reset the current Junction type for next cycle
                    currentJunctionType = JunctionType.WaitingForNewValue;
                    // Reset the Player has decided flag
                    playerHasDecidedOnRoute = false;
                    // Go back moving on the new rail
                    currentState = PlayerState.MovingAlongRail;                    
                }
            
            break;
            case PlayerState.FirstPersonAiming:

            break;
        }
    }

    private void HandleState()
    {
        
    }
}
