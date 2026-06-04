using UnityEngine;
using UnityEngine.Splines;

public class PlayerRailMovement : MonoBehaviour
{
    [SerializeField] private SplineContainer rail;
    [SerializeField] private float speed = 1f;

    private float railLength;
    private float distancePercentage;
    private float movementDirection = 1f;

    private Vector3 currentPosition;
    private void Start()
    {
        // This is the actual length in "meters" not a percentage!
        railLength = rail.CalculateLength();
        //Debug.Log(railLength);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            distancePercentage += movementDirection * speed * Time.deltaTime / railLength;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movementDirection *= -1f;
        }

        // Stopping/Getting stuck at the end at the end
        if (distancePercentage > 1f)
        {
            distancePercentage = 1f;
        }
        // Also cant go past the start
        if (distancePercentage < 0f)
        {
            distancePercentage = 0f;
        }

        currentPosition = rail.EvaluatePosition(distancePercentage);
        Vector3 forward = rail.EvaluateTangent(distancePercentage);

        if (movementDirection == -1f) {
            forward = -forward;
        }
        
        transform.position = currentPosition;
        transform.forward = forward;
    }
}
