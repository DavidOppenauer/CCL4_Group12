using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] private RailSegment initialRail;
    private RailSegment currentRail;
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
                        currentState = PlayerState.ChoosingNextRail;
                    } 
                    else
                    {
                        return;
                    }
                }
            break;
            case PlayerState.ChoosingNextRail:
                Debug.Log("We here in Choosong");
            break;
            case PlayerState.FirstPersonAiming:

            break;
        }
    }

    private void HandleState()
    {
        
    }
}
