using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopChase : MonoBehaviour
{
    public Transform player;
    public Transform goblin;

    public float screenLimitX = 10f; // Adjust this to match the width of your screen
    public float playerSpeed = 5f;
    public float goblinSpeed = 5f;
    public float chaseSwitchInterval = 3f; // Interval in seconds before chase role changes

    private bool isGoblinChasing = true;

    void Start()
    {
        StartCoroutine(SwitchChaseRole());
    }

    void Update()
    {
        HandleLoopPosition(player, playerSpeed);
        HandleLoopPosition(goblin, goblinSpeed);
        HandleChaseDirection();
    }

    void HandleLoopPosition(Transform character, float speed)
    {
        // Move character to the right
        character.Translate(Vector2.right * speed * Time.deltaTime);

        // Loop character to the left side if they go off screen to the right
        if (character.position.x > screenLimitX)
        {
            character.position = new Vector2(-screenLimitX, character.position.y);
        }
    }

    void HandleChaseDirection()
    {
        if (isGoblinChasing)
        {
            goblinSpeed = playerSpeed + 1f; // Goblin slightly faster when chasing
        }
        else
        {
            playerSpeed = goblinSpeed + 1f; // Player slightly faster when being chased
        }
    }

    IEnumerator SwitchChaseRole()
    {
        while (true)
        {
            yield return new WaitForSeconds(chaseSwitchInterval);
            isGoblinChasing = !isGoblinChasing;

            // Randomize interval for next chase switch for unpredictability
            chaseSwitchInterval = Random.Range(2f, 5f);
        }
    }
}