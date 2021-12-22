using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
    Player player;
    EnemyStateManager enemy;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        enemy = GameObject.FindObjectOfType<EnemyStateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player, enemy, or other objects enter the deadtrigger they either die or get destroyed
        if (other.CompareTag("Player")) player.takeDamage(player.healtBar.getHealth());
        else if (other.CompareTag("Enemy")) enemy.takeDamage(enemy.healt);
        else Destroy(other.gameObject);
    }
}
