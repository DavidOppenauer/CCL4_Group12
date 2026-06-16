using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    private int GetCurrentHealth()
    {
        return currentHealth;
    }
}