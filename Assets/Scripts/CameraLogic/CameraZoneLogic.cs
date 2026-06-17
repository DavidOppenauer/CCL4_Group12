using Unity.Cinemachine;
using UnityEngine;

public class CameraZoneLogic : MonoBehaviour
{
    [SerializeField] CinemachineCamera cameraForThisZone;
    [SerializeField] CameraController cameraController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            cameraController.SetCurrentRailCamera(cameraForThisZone);
        }
    }
}
