using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHealthStatus : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image healthBar;
    private HealthSystem playerHealthSystem;
    private float startHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealthSystem = player.GetComponentInChildren<HealthSystem>();
        startHealth = playerHealthSystem.GetCurrentHealth();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("player current health = " + playerHealthSystem.GetCurrentHealth() + "player max health = " + startHealth + "health percentage = " + playerHealthSystem.GetCurrentHealth() / startHealth);
        healthBar.fillAmount = playerHealthSystem.GetCurrentHealth() / startHealth;
    }
}
