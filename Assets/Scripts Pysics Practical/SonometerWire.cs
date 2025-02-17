using UnityEngine;

public class SonometerWire : MonoBehaviour
{

    public Transform startPoint;
    public Transform bridge1;
    public Transform bridge2;
    public Transform pulley;
    public Transform weightPoint;

    private LineRenderer lineRenderer;

    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 5;

        UpdateWire();
    }

    void UpdateWire()
    {

        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, bridge1.position);
        lineRenderer.SetPosition(2, bridge2.position);
        lineRenderer.SetPosition(3, pulley.position);
        lineRenderer.SetPosition(4, weightPoint.position);
    }

    void Update()
    {
        UpdateWire();
    }
}