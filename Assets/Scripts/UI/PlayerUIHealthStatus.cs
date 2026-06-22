using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHealthStatus : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = GameManager.Instance.playerHealth / GameManager.Instance.playerMaxHealth;
    }
}
