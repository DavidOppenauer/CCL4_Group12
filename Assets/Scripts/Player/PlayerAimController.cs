using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    // Animation Stuff
    [Header("Player Visual")]
    [SerializeField] private Transform playerVisual;
    [SerializeField] private Vector3 aimingVisualPositionOffset;
    [SerializeField] private Vector3 aimingVisualRotationOffset;
    private Vector3 defaultVisualLocalPosition;
    private Quaternion defaultVisualLocalRotation;

    [Header("Visual Screen Pinning")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform aimPinPoint;
    [SerializeField] private float pinDistanceFromCamera = 1.5f;
    [SerializeField] private Vector3 pinOffsetInCameraSpace;

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
    {   /*
        // Start aligned with whatever rotation the pivot already has in the scene.
        horizontalRotation = aimCameraPivot.localEulerAngles.y;
        verticalRotation = aimCameraPivot.localEulerAngles.x;*/
        defaultVisualLocalPosition = playerVisual.localPosition;
        defaultVisualLocalRotation = playerVisual.localRotation;

        ResetCamera();
        ResetPlayerVisual();
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

        ResetCamera();
        ResetPlayerVisual();
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

        // First put the visual into its basic aiming pose.
        playerVisual.localPosition = defaultVisualLocalPosition + aimingVisualPositionOffset;
        playerVisual.localRotation = Quaternion.Euler(
        verticalRotation + aimingVisualRotationOffset.x,
        horizontalRotation + aimingVisualRotationOffset.y,
        aimingVisualRotationOffset.z);

        // Then move the whole visual so the AimPinPoint stays pinned to the center of the screen.
        PinVisualToCrosshair();
    }

    private void PinVisualToCrosshair()
    {
        if (mainCamera == null || aimPinPoint == null || playerVisual == null)
        {
            return;
        }

        Vector3 targetPoint = mainCamera.ViewportToWorldPoint(
            new Vector3(0.5f, 0.5f, pinDistanceFromCamera)
        );

        targetPoint += mainCamera.transform.TransformDirection(pinOffsetInCameraSpace);

        Vector3 correction = targetPoint - aimPinPoint.position;

        playerVisual.position += correction;
    }

    public void ResetPlayerVisual()
    {
        playerVisual.localPosition = defaultVisualLocalPosition;
        playerVisual.localRotation = defaultVisualLocalRotation;
    }

    private void ResetCamera()
    {
        horizontalRotation = 0f;
        verticalRotation = 0f;
        aimCameraPivot.localRotation = Quaternion.identity;
    }
}