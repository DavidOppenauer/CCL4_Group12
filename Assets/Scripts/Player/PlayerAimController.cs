using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField] private PlayerInputs playerInput;

    [Header("Aim Objects")]
    [SerializeField] private Transform aimCameraPivot;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minVerticalLook = -45f;
    [SerializeField] private float maxVerticalLook = 45f;

    private float horizontalRotation;
    private float verticalRotation;

    private bool aimingIsActive;

    private void Start()
    {
        // Start aligned with whatever rotation the pivot already has in the scene.
        horizontalRotation = aimCameraPivot.localEulerAngles.y;
        verticalRotation = aimCameraPivot.localEulerAngles.x;
    }

    public void EnableAiming()
    {
        aimingIsActive = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableAiming()
    {
        aimingIsActive = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HandleAiming()
    {
        if (!aimingIsActive)
        {
            return;
        }

        Vector2 lookInput = playerInput.GetLookInput();

        horizontalRotation += lookInput.x * mouseSensitivity;
        verticalRotation -= lookInput.y * mouseSensitivity;

        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalLook, maxVerticalLook);

        aimCameraPivot.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }
}