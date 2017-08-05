using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBunny : Character
{
    public float timeBetweenAttacks = .5f;
    public int attackDamage = 10;
    public Post shellPost;
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
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        playerCharacter = GetComponent<PlayerCharacter>();

        shellPost.isPostBlue = true;
        isBlue = true;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 3)
        {
            timer = 0f;
            Shoot();
        }
    }


    void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            if (playerCharacter.isBlue == true)
            {
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
