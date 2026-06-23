using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [SerializeField] private Gradient chargeUpColors;
    [SerializeField] private float chargeUpTime = .5f;
    [SerializeField] private GameObject explosionPrefab;

    // private MeshRenderer _meshRenderer;

    protected override void Start()
    {
        base.Start();
        // _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        animator.SetTrigger("Attack");
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