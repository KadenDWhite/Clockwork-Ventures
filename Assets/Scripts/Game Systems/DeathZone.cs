using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene reloading

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Reload the current scene if the player falls into the death zone
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Check if the object entering the death zone is an enemy
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHP enemyHP = other.GetComponent<EnemyHP>();

            if (enemyHP != null)
            {
                // Instantly kill the enemy by setting its HP to 0
                enemyHP.currentHP = 0;
                enemyHP.animator.SetTrigger("Hurt"); // Trigger any hurt animation or feedback

                // Trigger the enemy's death if it's not already dead
                if (!enemyHP.animator.GetBool("IsDead"))
                {
                    enemyHP.Die();
                }
            }
        }
    }
}