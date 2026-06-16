using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}