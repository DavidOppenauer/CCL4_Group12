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
    [SerializeField] CinemachineCamera aimCamera;
    [SerializeField] CinemachineCamera initialRailCamera;
    public void SwitchToRailCamera()
    {
        aimCamera.Priority = 0;
        initialRailCamera.Priority = 10;
    }

    public void SwitchToAimCamera()
    {
        initialRailCamera.Priority = 0;
        aimCamera.Priority = 10;
    }

}
