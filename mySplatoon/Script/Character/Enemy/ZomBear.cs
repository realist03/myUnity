using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBear : Character
{
    public float timeBetweenAttacks = .5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;

    public bool canDamage;


    private PlayerCharacter playerCharacter;

    void Awake()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        playerCharacter = GetComponent<PlayerCharacter>();

        isBlue = false;
    }


    void OnCollisionEnter(Collision collision)
    {
        // If the entering collider is the player...
        if (collision.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
        }
    }


    void OnCollisionExit(Collision collision)
    {
        // If the exiting collider is the player...
        if (collision.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        if (timer >= 5)
        {
            timer = 0f;
            Shoot();
        }

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        //if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        //{
        //    // ... attack.
        //    Attack();
        //}

        //// If the player has zero or less health...
        //if (playerHealth.currentHealth <= 0)
        //{
        //    // ... tell the animator the player is dead.
        //    anim.SetTrigger("PlayerDead");
        //}
    }


    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose...
        if (playerHealth.currentHealth > 0)
        {
            if (playerCharacter.isBlue == true)
            {
                // ... damage the player.
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    public void Shoot()
    {
        Rigidbody ball;
        ParticleSystem shoot;
        ball = Instantiate(shell, gun.position, gun.rotation);
        shoot = Instantiate(fireFX, gun.position, gun.rotation);
        ball.gameObject.SetActive(true);
        shoot.gameObject.SetActive(true);
        ball.AddForce(transform.forward * fireForce);
        Destroy(ball.gameObject, 5);
        Destroy(shoot.gameObject, 3);
        fireFX.gameObject.SetActive(true);
    }

}
