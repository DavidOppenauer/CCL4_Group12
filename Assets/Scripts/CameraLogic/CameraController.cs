using Unity.Cinemachine;
using Unity.VisualScripting;
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

    private void Start()
    {
        currentRailCamera = initialRailCamera;
    }
    public void SwitchToCurrentRailCamera()
    {
        currentRailCamera.Priority = 10;
        previousRailCamera.Priority = 0;
        aimCamera.Priority = 0;
        reloadCamera.Priority = 0;
    }

    public void SwitchToAimCamera()
    {
        aimCamera.Priority = 10;
        initialRailCamera.Priority = 0;
        reloadCamera.Priority = 0;
    }

    public void SwitchToReloadCamera()
    {
        reloadCamera.Priority = 10;
        initialRailCamera.Priority = 0;
        aimCamera.Priority = 0;
    }

    public void SetCurrentRailCamera(CinemachineCamera _currentCamera)
    {
        previousRailCamera = currentRailCamera;
        currentRailCamera = _currentCamera;
        SwitchToCurrentRailCamera();
    }
    
}
