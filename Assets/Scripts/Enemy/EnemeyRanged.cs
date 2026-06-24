using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemeyRanged : EnemyBase
{
    [SerializeField] private Transform firePoint;
    // [SerializeField] private Gradient chargeUpColors;
    [SerializeField] private float chargeUpTime = 1.5f;
    [SerializeField] private float laserDuration = .5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float stunTime = 1f;
    [SerializeField] private float baseAnimationLength = 2.1f;

    private LineRenderer _lineRenderer;
    // private MeshRenderer _meshRenderer;
    private float timer = 0f;
    private Coroutine activeCoroutine;
    private bool hasAnnounced = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        // _meshRenderer = GetComponentInChildren<MeshRenderer>();

    }

    protected override void Update()
    {
        base.Update();
        if (currentState == EnemyState.Chasing && !hasAnnounced)
        {
            hasAnnounced = true; // Lock the padlock so it never runs again!

            AkUnitySoundEngine.PostEvent("Play_Angel_Announce", gameObject);
        }
    }

    protected override void Attack()
    {
        if (activeCoroutine == null)
        {
            animator.SetTrigger("Charge");
            activeCoroutine = StartCoroutine(LaserSequence());
        }

    }

    public override void OnHit()
    {
        healthSystem.TakeDamage(1);

        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(StunSequence());

        if (healthSystem.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator LaserSequence()
    {
        timer = 0f;
        // Laser charge-up effect
        float calculatedSpeed = baseAnimationLength / chargeUpTime;
        animator.SetFloat("ChargeSpeedMultiplier", calculatedSpeed);
        animator.SetTrigger("Charge");

        AkUnitySoundEngine.PostEvent("Play_Angel_ChargeUP", gameObject);
        while (timer < chargeUpTime)
        {
            timer += Time.deltaTime;

            // float chargeUpPercentage = timer / chargeUpTime;

            // _meshRenderer.material.color = chargeUpColors.Evaluate(chargeUpPercentage);

            yield return null;
        }
        AkUnitySoundEngine.PostEvent("Stop_Stun_Angel", gameObject);
        animator.SetTrigger("Attack");

        // Laser effect
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, firePoint.position);
        _lineRenderer.SetPosition(1, player.transform.position);
        yield return new WaitForSeconds(laserDuration);

        // Explosion effect
        Instantiate(explosionPrefab, firePoint.position, Quaternion.identity);
        AkUnitySoundEngine.PostEvent("Play_Explosion", gameObject);

        // Damage the player
        PlayerBrain playerBrain = player.GetComponent<PlayerBrain>();
        playerBrain.OnHit();
        Destroy(gameObject);
    }
    private IEnumerator StunSequence()
    {
        timer = 0f;

        AkUnitySoundEngine.PostEvent("Stop_Stun_Angel", gameObject);

        if (animator != null) animator.SetTrigger("Stun");

        yield return new WaitForSeconds(stunTime);

        activeCoroutine = null;
    }
}
