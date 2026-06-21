using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [SerializeField] private Gradient chargeUpColors;
    [SerializeField] private float chargeUpTime = .5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private bool isFlying;
    [SerializeField]private float targetOffset = 10f;

   private MeshRenderer _meshRenderer;
    private float defaultHeight;
    private float startingDistance;
    private bool hasCalculatedStartDistance = false;

    protected override void Start()
    {
        base.Start();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        defaultHeight = _meshRenderer.transform.localPosition.y;

    }

    protected override void Update()
    {
        base.Update();
        
        if (isFlying && _meshRenderer != null)
        {
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
                    
                Vector3 newPos = _meshRenderer.transform.localPosition;
                newPos.y = Mathf.Lerp(playerHead, defaultHeight, percentage);
                _meshRenderer.transform.localPosition = newPos;
            }
        }
    }
    protected override void Attack()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        float timer = 0f;

        while (timer < chargeUpTime)
        {
            
            timer += Time.deltaTime;
            float chargeUpPercentage = timer / chargeUpTime;

            _meshRenderer.material.color = chargeUpColors.Evaluate(chargeUpPercentage);
            yield return null;
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        PlayerBrain playerBrain = player.GetComponent<PlayerBrain>();
        playerBrain.OnHit();
        Destroy(gameObject);
    }
}