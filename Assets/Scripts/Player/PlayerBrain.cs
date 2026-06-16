using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    // Global variable for inactivity during reload
    private float timer = 0f;

    // New Input System
    [SerializeField] private PlayerInputs playerInput;

    // Aiming controlls
    private PlayerAimController playerAimController;

    // Switching the camera, just between 2 the clearer distinction happens in cameraController
    [SerializeField] private CameraController cameraController;
    [SerializeField] private RailSegment initialRail;

    // ------- UI REFERENCES -------
    [SerializeField] private GameObject reloadUI;
    
    // Rail variable for MovementState
    private RailSegment currentRail;
    // Rail variable for choosing state for more clarity
    private RailSegment chosenRail;
    private RailJunction currentJunction;
    private PlayerRailMovement playerRailMovement;
    private RailSegment[] connectedRailsToCurrentJunction;
    private List<RailSegment> availableRailChoices;
    private int selectedRailChoiceIndex;
    private bool playerHasDecidedOnRoute = false;
    private void Awake()
    {
        playerRailMovement = GetComponent<PlayerRailMovement>();
        playerAimController = GetComponent<PlayerAimController>();
        currentState = PlayerState.MovingAlongRail;
        currentRail = initialRail;
    }
    private enum PlayerState
    {
        MovingAlongRail,
        ChoosingNextRail,
        Aiming,
        Reload
    }
    private PlayerState currentState;
    private PlayerState previousState;

    private void Update()
    {
        HandleState();
        switch (currentState)
        {
            case PlayerState.MovingAlongRail:
                
                // Transition Code
                if(previousState != currentState)
                {
                    if (previousState == PlayerState.Aiming)
                    {
                        // First to third person Transition changes
                        // HandleAimingToMoving();   
                    }
                    playerAimController.DisableAiming();
                    cameraController.SwitchToRailCamera();
                    previousState = currentState;
                }

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
                    // Get the array of Rails(I aint changing it now because its already wired up in the editor(too much work))
                    connectedRailsToCurrentJunction = currentJunction.GetRailSegments();
                    // Convert said Array into a list
                    availableRailChoices = connectedRailsToCurrentJunction.ToList();
                    // reset the choiceindex
                    selectedRailChoiceIndex = 0;
                    previousState = currentState;                  
                }

                // ---------------------------------------------------------------------------------
                // THIS NEXT PART IS REALLY EXPERIMENTAL AND NEEDS GOOD CARE AND THOUGHT LATER ON...
                // ---------------------------------------------------------------------------------

                // Setting currentRail to what the player chooses
                // S should always be the return choice
                if(playerInput.GetTurnAroundWasPressedThisFrame())// Go back
                {
                    foreach ( RailSegment rail in availableRailChoices)
                    {
                        if(rail == currentRail)
                        {
                            chosenRail = currentRail;
                        }
                    }
                    playerHasDecidedOnRoute = true;
                }
                // REFACTOR OF NEW INPUT SYSTEM INCOMPLETE BECAUSE THIS SECTION SHOULDNT EXISSSSST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if(Input.GetKeyDown(KeyCode.W))// Confirm
                {
                    chosenRail = availableRailChoices[selectedRailChoiceIndex];
                    playerHasDecidedOnRoute = true;
                }
                if(Input.GetKeyDown(KeyCode.A))// Cycle left
                {
                    selectedRailChoiceIndex -= 1;
                    if (selectedRailChoiceIndex < 0)
                    {
                        selectedRailChoiceIndex = availableRailChoices.Count - 1;
                    }
                    Debug.Log("" + selectedRailChoiceIndex);
                }
                if(Input.GetKeyDown(KeyCode.D))// Cycle right
                {
                    selectedRailChoiceIndex += 1;
                    if (selectedRailChoiceIndex >= availableRailChoices.Count)
                    {
                        selectedRailChoiceIndex = 0;
                    }
                    Debug.Log("" + selectedRailChoiceIndex);
                }
                if (playerHasDecidedOnRoute == true)
                {
                    // Look if the Junction the player is currently at is at the end or the start of the chosen junction, then place the player accordingly
                    if(currentJunction == chosenRail.GetStartJunction())
                    {
                        playerRailMovement.PlaceOnRailStart();
                    }
                    if (currentJunction == chosenRail.GetEndJunction())
                    {
                        playerRailMovement.PlaceOnRailEnd();
                    }
                    // After everythings done reset the current Junction type for next cycle
                    // Set the new current Rail
                    currentRail = chosenRail;
                    // Reset the Player has decided flag
                    playerHasDecidedOnRoute = false;
                    // Go back moving on the new rail
                    currentState = PlayerState.MovingAlongRail;                    
                }
            
            break;
            case PlayerState.Aiming:
                /*
                    show crosshair
                    hide player show gun
                */
                // Transition Code
                if(previousState != currentState)
                {
                    playerAimController.EnableAiming();
                    cameraController.SwitchToAimCamera();
                    previousState = currentState;
                }

                if (playerInput.GetReloadWasPressedThisFrame())
                {
                    playerAimController.DisableAiming();
                    currentState = PlayerState.Reload;
                }
                playerAimController.HandleAiming();
                
            break;
            case PlayerState.Reload:
                
                // Transition Code
                if(previousState != currentState)
                {
                    //playerAimController.EnableAiming();
                    cameraController.SwitchToReloadCamera();
                    reloadUI.SetActive(true);
                    previousState = currentState;
                }
                // ---- Timer section -----
                // Used for inactivity during Reload state 
                // Around 2.5 seconds, so it matches the Animations in the Reload_UI
                timer += Time.deltaTime;

                if (timer >= 2.5f)
                {

                    timer = 0f;
                    reloadUI.SetActive(false);
                    currentState = PlayerState.MovingAlongRail;
                } 
                //playerAimController.HandleAiming();
                
            break;
        }
    }


    private void HandleState()
    {
        bool aimIsPressed = playerInput.GetAimIsPressed();

        if (aimIsPressed && currentState != PlayerState.ChoosingNextRail && currentState != PlayerState.Reload)
        {
            currentState = PlayerState.Aiming;
        }

        if (!aimIsPressed && currentState == PlayerState.Aiming && currentState != PlayerState.Reload)
        {
            currentState = PlayerState.MovingAlongRail;
        }

    }
}
