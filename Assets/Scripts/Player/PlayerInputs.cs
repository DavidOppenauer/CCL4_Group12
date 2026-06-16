using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{

    // Ill have this on a seperate Object so I you can use it for UI as well, so you dont need a player

    private PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.PlayerMovementMap.Enable();
        playerInput.PlayerCameraMap.Enable();
    }

    private void OnDisable()
    {
        playerInput.PlayerMovementMap.Disable();
        playerInput.PlayerCameraMap.Disable();
    }

    // Equivalent to old Input.GetKey(KeyCode.W)
    public bool GetMoveForwardIsPressed()
    {
        return playerInput.PlayerMovementMap.WalkForward.IsPressed();
    }

    // Equivalent to old Input.GetKeyDown(KeyCode.S)
    public bool GetTurnAroundWasPressedThisFrame()
    {
        return playerInput.PlayerMovementMap.TurnAround.WasPressedThisFrame();
    }
    // Shooting
    public bool GetShootWasPressedThisFrame()
    {
        return playerInput.PlayerMovementMap.Shoot.WasPressedThisFrame();
    }
    // Reload
    public bool GetReloadWasPressedThisFrame()
    {
        return playerInput.PlayerMovementMap.Reload.WasPressedThisFrame();
    }

    // Equivalent to old Input.GetKeyDown(KeyCode.Space), if you have an Aim action
    public bool GetAimIsPressed()
    {
        return playerInput.PlayerMovementMap.AimMode.IsPressed();
    }

    // Mouse look input, if you have a Look action as Vector2
    public Vector2 GetLookInput()
    {
        return playerInput.PlayerCameraMap.Look.ReadValue<Vector2>();
    }
}