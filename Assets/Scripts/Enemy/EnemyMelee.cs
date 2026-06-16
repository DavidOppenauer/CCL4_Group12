using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [SerializeField] private Gradient chargeUpColors;
    [SerializeField] private float chargeUpTime = .5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private bool isFlying;
    private MeshRenderer _meshRenderer;
    private float defaultHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        defaultHeight = _meshRenderer.transform.localPosition.y;
    }

    protected override void Update()
    {
        base.Update();
        if (isFlying)
        {
            float distanceToPlayer = Vector3.Distance(transform.position,player.transform.position);

            float heightPercentage = Mathf.InverseLerp(attackRange, triggerRange, distanceToPlayer);

           _meshRenderer.transform.localPosition = new Vector3(_meshRenderer.transform.localPosition.x, defaultHeight * heightPercentage, _meshRenderer.transform.localPosition.z);
            
        }
    }
    protected override void Attack()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        float timer = 0f;

        while(timer < chargeUpTime)
        {
            timer += Time.deltaTime;
            float chargeUpPercentage = timer / chargeUpTime;

            _meshRenderer.material.color = chargeUpColors.Evaluate(chargeUpPercentage);
            yield return null;
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
