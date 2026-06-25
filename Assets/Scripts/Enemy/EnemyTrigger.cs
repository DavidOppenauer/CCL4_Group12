using UnityEngine;
using System.Collections.Generic;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private List<EnemyBase> enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemies != null && enemies.Count > 0)
            {
                foreach (var enemy in enemies)
                {
                    enemy.TriggerEnemy();
                }
            }
        }
    }
}
