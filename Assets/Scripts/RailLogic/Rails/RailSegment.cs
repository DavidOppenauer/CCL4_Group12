using UnityEngine;
using UnityEngine.Splines;

public class RailSegment : MonoBehaviour
{
    [SerializeField] private RailJunction startJunction;
    [SerializeField] private RailJunction endJunction;
    private SplineContainer railSegment;

    private void Awake()
    {
        railSegment = GetComponent<SplineContainer>();
    }

    public SplineContainer GetSplineContainer()
    {
        return railSegment;
    }

    public RailJunction GetStartJunction()
    {
        return startJunction;
    }

    public RailJunction GetEndJunction()
    {
        return endJunction;
    }

}
