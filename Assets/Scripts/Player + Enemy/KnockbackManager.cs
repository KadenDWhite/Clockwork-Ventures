using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackManager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float strength = 16, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;

    private PlayerHP playerHP; // Reference to the PlayerHP script
    private EnemyHP enemyHP;   // Reference to the EnemyHP script

    void Start()
    {
        playerHP = GetComponent<PlayerHP>(); // Initialize the PlayerHP reference
        enemyHP = GetComponent<EnemyHP>();   // Initialize the EnemyHP reference
    }

    public void PlayFeedback(GameObject sender)
    {
        // Ensure no knockback occurs if the player or enemy is dead
        if (playerHP != null && playerHP.IsDead()) return;
        if (enemyHP != null && enemyHP.IsDead()) return;

        StopAllCoroutines();
        OnBegin?.Invoke();

        // Direction of knockback based on sender's position
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb2d.AddForce(direction * strength, ForceMode2D.Impulse);

        // Reset knockback after a delay
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector3.zero;

        // Ensure OnDone only happens if the player or enemy is still alive
        if (playerHP != null && !playerHP.IsDead() || enemyHP != null && !enemyHP.IsDead())
        {
            OnDone?.Invoke();
        }
    }
}