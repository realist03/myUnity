using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    public float fireForce = 1000;
    public Rigidbody shell;
    public Transform gun;
    public ParticleSystem fireFX;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
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

        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
        }

        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("PlayerDead");
        }
    }


    void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
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
