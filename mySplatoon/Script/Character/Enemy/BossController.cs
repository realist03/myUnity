using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    public Material redShellM;
    public Material blueShellM;
    //public Renderer playerRenderer;
    public Renderer shellRenderer;
    public SpriteRenderer post;
    public ParticleSystem redFire;
    public ParticleSystem blueFire;
    public ParticleSystem shellFX;
    Color blue = Color.HSVToRGB(0.52f, 0.674f, 1);
    Color red = Color.HSVToRGB(0, 0.368f, 1);

    Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;

    public float fireForce = 1000;
    public Rigidbody shell;
    public Transform gun;
    public Transform gun2;
    public Transform gun3;
    public ParticleSystem fireFX;

    private PlayerCharacter playerCharacter;
    private bool isBlue = true;

    void Awake()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        playerCharacter = GetComponent<PlayerCharacter>();
    }


    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;


        if (timer >= 3)
        {
            timer = 0f;
            Shoot();
            TransColor();
        }

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        //if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        //{
        //    // ... attack.
        //    Attack();
        //}

        // If the player has zero or less health...
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
            // ... damage the player.
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void Shoot()
    {
        Rigidbody ball;
        Rigidbody ball1;
        Rigidbody ball2;
        ParticleSystem shoot;
        ParticleSystem shoot1;
        ParticleSystem shoot2;
        ball = Instantiate(shell, gun.position, gun.rotation);
        shoot = Instantiate(fireFX, gun.position, gun.rotation);
        ball1 = Instantiate(shell, gun2.position, gun2.rotation);
        shoot1 = Instantiate(fireFX, gun2.position, gun2.rotation);
        ball2 = Instantiate(shell, gun3.position, gun3.rotation);
        shoot2 = Instantiate(fireFX, gun3.position, gun3.rotation);
        ball.gameObject.SetActive(true);
        shoot.gameObject.SetActive(true);
        ball1.gameObject.SetActive(true);
        shoot1.gameObject.SetActive(true);
        ball2.gameObject.SetActive(true);
        shoot2.gameObject.SetActive(true);
        ball.AddForce(transform.forward * fireForce);
        ball1.AddForce(transform.forward * fireForce);
        ball2.AddForce(transform.forward * fireForce);
        Destroy(ball.gameObject, 5);
        Destroy(shoot.gameObject, 3);
        Destroy(ball1.gameObject, 5);
        Destroy(shoot1.gameObject, 3);
        Destroy(ball2.gameObject, 5);
        Destroy(shoot2.gameObject, 3);
        fireFX.gameObject.SetActive(true);
    }

    public void TransColor()
    {
        if (isBlue)
        {
            isBlue = false;

            fireFX = redFire;
            //playerRenderer.material = redPlayer;
            shellRenderer.material = redShellM;
            post.color = red;
            //ParticleSystem.MainModule main = shellFX.main;
            //main.startColor = red;
        }
        else
        {
            isBlue = true;

            fireFX = blueFire;
            //playerRenderer.material = bluePlayer;
            shellRenderer.material = blueShellM;
            post.color = blue;
            //ParticleSystem.MainModule main = shellFX.main;
            //main.startColor = blue;
        }
    }

}
