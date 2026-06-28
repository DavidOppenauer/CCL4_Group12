using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;

    private void Awake()
    {
        // Important: targetFrameRate is ignored if VSync is enabled.
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = targetFrameRate;
    }
}