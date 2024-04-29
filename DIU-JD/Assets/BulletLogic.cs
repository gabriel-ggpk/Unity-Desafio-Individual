using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) return;
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // If the Player component is found, call TakeDamage
            player.TakeDamage();  // Assuming you want to deal 10 damage
        }

        Destroy(gameObject);
    }
}
