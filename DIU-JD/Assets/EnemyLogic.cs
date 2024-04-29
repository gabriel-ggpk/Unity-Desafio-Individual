using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private GameObject player;
    public float bulletSpeed = 10f;
    [SerializeField] GameObject bulletPrefab;
    private float shotCooldown = 2f; // Cooldown time in seconds
    private float shotTimer;
    [SerializeField] Animator animator;


    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        shotTimer = shotCooldown;
    }

    void Update()
    {
        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0f)
        {
            shotTimer = shotCooldown;
            TriggerShotAnimation(); // This should trigger the animation
        }
    }

    private void TriggerShotAnimation()
    {
        animator.SetTrigger("Shooting");
    }

    public void FireBullet() // This will be called by the Animation Event
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
