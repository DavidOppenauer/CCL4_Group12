using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] private RailSegment initialRail;
    private RailSegment currentRail;
    private RailJunction currentJunction;
    private PlayerRailMovement playerRailMovement;
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

    private void Update()
    {
        Debug.Log(currentState);
        HandleState();
        switch (currentState)
        {
            case PlayerState.MovingAlongRail:
                playerRailMovement.HandleMovement(currentRail);
                if (playerRailMovement.GetEndOfRailReached())
                {
                    if (currentRail.GetEndJunction() != null)
                    {
                        currentJunction = currentRail.GetEndJunction();
                        // Stop early if theres just the one rail its connected to its a dead end
                        playerRailMovement.ResetJunctionFlags();
                        if (currentJunction.GetRailCount() == 1)
                        {
                            return;
                        }
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
                        currentState = PlayerState.ChoosingNextRail;
                    } 
                    else
                    {
                        return;
                    }
                }
            break;
            case PlayerState.ChoosingNextRail:
                

            break;
            case PlayerState.FirstPersonAiming:

            break;
        }
    }

    private void HandleState()
    {
        
    }
}
