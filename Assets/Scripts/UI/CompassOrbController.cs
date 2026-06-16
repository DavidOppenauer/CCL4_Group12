using UnityEngine;

public class CompassOrbController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    [Tooltip("The RectTransform of the small 2D indicator orb.")]
    [SerializeField] private RectTransform orbRectTransform;

    [Header("Settings")]
    [Tooltip("How far away from the center of the circle the orb should orbit (in UI pixels).")]
    [SerializeField] private float orbitRadius = 75f;

    void Start()
    {
        if (playerTransform == null && Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }
    }
    
    void LateUpdate()
    {
    if (playerTransform == null || orbRectTransform == null) return;

        float playerYAngle = playerTransform.eulerAngles.y;


        float radians = playerYAngle * Mathf.Deg2Rad;

        float newX = Mathf.Sin(radians) * orbitRadius;
        float newY = Mathf.Cos(radians) * orbitRadius;

        orbRectTransform.anchoredPosition = new Vector3(newX, newY, 0f);
    }
}