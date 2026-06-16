using Unity.VisualScripting;
using UnityEngine;

public class EnemeyRanged : EnemyBase
{
    private LineRenderer _lineRenderer;
    private Ray _ray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    protected override void Attack()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, player.transform.position);
        WaitForSeconds wait = new WaitForSeconds(20f);
    }
}
