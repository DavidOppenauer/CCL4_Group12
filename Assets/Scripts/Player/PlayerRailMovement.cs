using UnityEngine;
using UnityEngine.Splines;

public class PlayerRailMovement : MonoBehaviour
{
    [SerializeField] private RailSegment initialRail;
    private RailSegment currentRail;
    [SerializeField] private float speed = 1f;

    private float railLength;
    private float distancePercentage;
    private float movementDirection = 1f;

    private Vector3 currentPosition;
    private void Start()
    {
        // This is the actual length in "meters" not a percentage!
        currentRail = initialRail;
        railLength = currentRail.GetSplineContainer().CalculateLength();
    }

    // Update is called once per frame
    public void HandleMovement()
    {
        if(Input.GetKey(KeyCode.W))
        {
            distancePercentage += movementDirection * speed * Time.deltaTime / railLength;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movementDirection *= -1f;
        }
        // Junction Logic
        // Stopping/Getting stuck at the end at the end
        if (distancePercentage > 1f)
        {
            distancePercentage = 1f;
            if (currentRail.GetEndJunction() != null)
            {
                RailJunction railEndChoices = currentRail.GetEndJunction();
                Debug.Log(railEndChoices.GetRailSegments());
            }
            
        }
        // Also cant go past the start
        if (distancePercentage < 0f)
        {
            distancePercentage = 0f;
        }

        currentPosition = currentRail.GetSplineContainer().EvaluatePosition(distancePercentage);
        Vector3 forward = currentRail.GetSplineContainer().EvaluateTangent(distancePercentage);

        if (movementDirection == -1f) {
            forward = -forward;
        }
        
        transform.position = currentPosition;
        transform.forward = forward;
    }

    //public void
}
