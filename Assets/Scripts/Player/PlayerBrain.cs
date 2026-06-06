using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    private PlayerRailMovement playerRailMovement;
    private void Start()
    {
        playerRailMovement = GetComponent<PlayerRailMovement>();
        currentState = PlayerState.MovingAlongRail;
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
                playerRailMovement.HandleMovement();
                //if()
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
