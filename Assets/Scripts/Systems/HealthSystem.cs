using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    private bool isPlayer;

    void Awake()
    {
        isPlayer = gameObject.CompareTag("Player");
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isPlayer)
        {
            GameManager.Instance.playerHealth -= damageAmount;
        }
        else
        {
            currentHealth -= damageAmount;
        }
    }

    public float GetCurrentHealth()
    {
        if (isPlayer)
        {
            return GameManager.Instance.playerHealth;
        }
        else
        {
            return currentHealth;
        }
    }
}