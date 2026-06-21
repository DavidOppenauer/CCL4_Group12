using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    // Interaction stuff
    [SerializeField] private PlayerInteractionDetector playerInteractionDetector;
    private String currentInteraction;
    // Global variable for inactivity during reload
    private float timer = 0f;
    
    // New Input System
    [SerializeField] private PlayerInputs playerInput;

    // UI controls for junction
    [SerializeField] private JunctionManager junctionManager;

    // Aiming controlls
    private PlayerAimController playerAimController;

    //Shooting
    private PlayerShoot playerShoot;

    // Switching the camera, just between 2 the clearer distinction happens in cameraController
    [SerializeField] private CameraController cameraController;
    [SerializeField] private RailSegment initialRail;

    // ------- UI REFERENCES -------
    [SerializeField] private GameObject reloadUI;
    [SerializeField] private GameObject aimUI;

    // SceneLoader for respawning
    [SerializeField] private SceneLoader sceneLoader;

    // Rail variable for MovementState
    private RailSegment currentRail;
    // Rail variable for choosing state for more clarity
    private RailSegment chosenRail;
    private RailJunction currentJunction;
    private PlayerRailMovement playerRailMovement;

    // Player also uses the general healthsystem
    protected HealthSystem healthSystem;

    // Different States of the player for the statemachine
    private enum PlayerState
    {
        MovingAlongRail,
        ChoosingNextRail,
        Aiming,
        Reload,
        Interaction
    }

    // These two are used for logic that should run only when first entering a state
    private PlayerState currentState;
    private PlayerState previousState;

    private void Awake()
    {
        playerRailMovement = GetComponent<PlayerRailMovement>();
        playerAimController = GetComponent<PlayerAimController>();
        playerShoot = GetComponent<PlayerShoot>();
        currentState = PlayerState.MovingAlongRail;
        currentRail = initialRail;
        healthSystem = GetComponentInChildren<HealthSystem>();
        // Event for the UI junction screen
        junctionManager.OnDirectionSelected += HandleJunctionDirectionSelected;
    }


    private void Update()
    {
        HandleState();
        switch (currentState)
        {
            case PlayerState.MovingAlongRail:
                
                // Transition Code
                if(previousState != currentState)
                {
                    // Disable Walking Animation
                    playerRailMovement.SetIsPlayerWalking(false);
                    aimUI.SetActive(false);
                    playerAimController.DisableAiming();
                    cameraController.SwitchToCurrentRailCamera();
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
                        // Stop early if theres just the one rail its connected to, its a dead end //--------------------------------------------------- Depricated...
                        if (currentJunction.GetRailCount() == 1)
                        {
                            return;
                        }
                        Debug.Log("Reached END of rail: " + currentRail.name + " at junction: " + currentRail.GetEndJunction().name);
                        EnterChoosingNextRail();
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
                        Debug.Log("Reached START of rail: " + currentRail.name + " at junction: " + currentRail.GetStartJunction().name);
                        EnterChoosingNextRail();
                    }
                }
            break;
            case PlayerState.ChoosingNextRail:
                if(previousState != currentState)
                {
                    // Disable Walking Animation
                    playerRailMovement.SetIsPlayerWalking(false);
                    previousState = currentState;
                }
            
                if (playerInput.GetTurnAroundWasPressedThisFrame())
                {
                    ConfirmChosenRail(currentRail);
                    junctionManager.CloseJunctionMenu();
                }
            break;
            case PlayerState.Aiming:
                // Transition Code
                if(previousState != currentState)
                {
                    // Disable Walking Animation
                    playerRailMovement.SetIsPlayerWalking(false);
                    playerAimController.EnableAiming();
                    cameraController.SwitchToAimCamera();
                    aimUI.SetActive(true);
                    previousState = currentState;
                }

                if (playerInput.GetReloadWasPressedThisFrame())
                {
                    playerAimController.DisableAiming();
                    currentState = PlayerState.Reload;
                }
                playerAimController.HandleAiming();
                playerShoot.HandleShooting();
            break;
            
            case PlayerState.Reload:
                
                // Transition Code
                if(previousState != currentState)
                {
                    // Disable Walking Animation
                    playerRailMovement.SetIsPlayerWalking(false);
                    aimUI.SetActive(false);
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
            break;

            case PlayerState.Interaction:
                if(previousState != currentState)
                {
                    // Disable Walking Animation
                    playerRailMovement.SetIsPlayerWalking(false);
                    //cameraController.SwitchToCustomCamera(currentInteraction.Camera)
                    reloadUI.SetActive(true);
                    previousState = currentState;
                }

            break;
        }
    }

    public virtual void OnHit()
    {
        healthSystem.TakeDamage(1);
        if (healthSystem.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
            sceneLoader.ReloadActiveScene();
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
        if (playerInteractionDetector.GetPlayerHasEnteredInteraction())
        {
            currentState = PlayerState.Interaction;
        }

    }

    private void HandleJunctionDirectionSelected(string direction)
    {
        if (currentState != PlayerState.ChoosingNextRail)
        {
            return;
        }

        RailSegment rail = currentJunction.GetRailForDirection(currentRail, direction);
        ConfirmChosenRail(rail);
    }

    private void EnterChoosingNextRail()
    {
        aimUI.SetActive(false);
        JunctionManager.JunctionConfig config = currentJunction.GetJunctionConfig(currentRail);

        
        Debug.Log("Opening junction menu at " + currentJunction.name + 
                " coming from rail " + currentRail.name +
                " | Left: " + config.allowLeft +
                " Straight: " + config.allowStraight +
                " Right: " + config.allowRight);

        junctionManager.OpenJunctionMenu(config);

        previousState = currentState;
        currentState = PlayerState.ChoosingNextRail;
    }

    private void ConfirmChosenRail(RailSegment rail)
    {
        if (rail == null)
        {
            Debug.LogWarning("Tried to confirm null rail.");
            return;
        }

        if (currentJunction == rail.GetStartJunction())
        {
            playerRailMovement.PlaceOnRailStart();
        }
        else if (currentJunction == rail.GetEndJunction())
        {
            playerRailMovement.PlaceOnRailEnd();
        }
        else
        {
            Debug.LogWarning("Chosen rail is not connected to current junction!");
            return;
        }

        currentRail = rail;
        previousState = currentState;
        currentState = PlayerState.MovingAlongRail;
    }
}
