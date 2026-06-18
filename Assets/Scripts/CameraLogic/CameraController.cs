using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*
        IMPORTANT!!!!!!!
        - Camera switch blends over 2 seconds by default for some reason
    */
    /*
        TO DO:
        ShowCrosshair()
        HideCrosshair()
        // More cameras
        - currentRailCamera
        - latestRailCamera
        - aimCamera
        - SetCurrentRailCamera(camera) -> Can be used anywhere for the cinematic shots
        - SwitchToCurrentRailCamera()
        - SwitchToAimCamera()
    */
    [Header("Unique Cameras")]
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private CinemachineCamera initialRailCamera;
    [SerializeField] private CinemachineCamera reloadCamera;
    [Header("EviromentalCameras")]
    //[SerializeField] private CinemachineCamera RailCamera_2;

    private CinemachineCamera currentRailCamera;
    private CinemachineCamera previousRailCamera;

    public void SwitchToCurrentRailCamera()
    {
        if (previousRailCamera != null)
        {
            previousRailCamera.Priority = 0;
        }

        aimCamera.Priority = 0;
        reloadCamera.Priority = 0;

        currentRailCamera.Priority = 10;
    }

    public void SwitchToAimCamera()
    {
        if (currentRailCamera != null)
        {
            currentRailCamera.Priority = 0;
        }

        reloadCamera.Priority = 0;
        aimCamera.Priority = 10;
    }

    public void SwitchToReloadCamera()
    {
        if (currentRailCamera != null)
        {
            currentRailCamera.Priority = 0;
        }

        aimCamera.Priority = 0;
        reloadCamera.Priority = 10;
    }

    public void SetCurrentRailCamera(CinemachineCamera newCamera)
    {
        if (newCamera == null)
        {
            return;
        }

        if (newCamera == currentRailCamera)
        {
            SwitchToCurrentRailCamera();
            return;
        }

        previousRailCamera = currentRailCamera;
        currentRailCamera = newCamera;

        SwitchToCurrentRailCamera();
    }
    
}
