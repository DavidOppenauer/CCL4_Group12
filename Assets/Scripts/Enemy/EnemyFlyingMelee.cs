using System.Collections;
using UnityEngine;

public class EnemyFlyingMelee : EnemyBase
{
    // [SerializeField] private Gradient chargeUpColors;
    [SerializeField] private float chargeUpTime = .5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float targetOffset = 10f;
    [SerializeField] private Transform enemyVisuals;

    private MeshRenderer _meshRenderer;
    private float defaultHeight;
    private float startingDistance;
    private bool hasCalculatedStartDistance = false;

    protected override void Start()
    {
        base.Start();
        // _meshRenderer = GetComponentInChildren<MeshRenderer>();
        defaultHeight = enemyVisuals.transform.localPosition.y;

    }

    protected override void Update()
    {
        base.Update();
        if (!hasCalculatedStartDistance && currentState == EnemyState.Chasing)
        {
            startingDistance = Vector3.Distance(transform.position, player.transform.position);
            hasCalculatedStartDistance = true;
        }

        if (hasCalculatedStartDistance)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float percentage = Mathf.InverseLerp(attackRange, startingDistance, distanceToPlayer);

            Collider playerCollider = player.GetComponentInChildren<Collider>();

            float playerHead = playerCollider.bounds.max.y + targetOffset;

            Vector3 newPos = enemyVisuals.transform.localPosition;
            newPos.y = Mathf.Lerp(playerHead, defaultHeight, percentage);
            enemyVisuals.transform.localPosition = newPos;
        }
    }
    protected override void Attack()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        // float timer = 0f;
        // while (timer < chargeUpTime)
        // {

        //     timer += Time.deltaTime;
        //     // float chargeUpPercentage = timer / chargeUpTime;

        //     // _meshRenderer.material.color = chargeUpColors.Evaluate(chargeUpPercentage);
        //     yield return null;
        // }
        yield return new WaitForSeconds(chargeUpTime);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        PlayerBrain playerBrain = player.GetComponent<PlayerBrain>();
        playerBrain.OnHit();
        Destroy(gameObject);
    }
}