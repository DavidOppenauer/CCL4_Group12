using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemeyRanged : EnemyBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Gradient chargeUpColors;
    [SerializeField] private float chargeUpTime = 1.5f;
    [SerializeField] private float laserDuration = .5f;
    [SerializeField] private GameObject explosionPrefab;
    private LineRenderer _lineRenderer;
    private MeshRenderer _meshRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    protected override void Attack()
    {
        StartCoroutine(LaserSequence());
    }

    private IEnumerator LaserSequence()
    {
        float timer = 0f;

        while (timer < chargeUpTime)
        {
            timer += Time.deltaTime;

            float chargeUpPercentage = timer / chargeUpTime;

            _meshRenderer.material.color = chargeUpColors.Evaluate(chargeUpPercentage);

            yield return null;
        }

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, firePoint.position);
        _lineRenderer.SetPosition(1, player.transform.position);
        yield return new WaitForSeconds(laserDuration);

        Instantiate(explosionPrefab, firePoint.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
