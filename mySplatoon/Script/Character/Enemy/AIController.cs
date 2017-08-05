using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    public AudioClip deathClip;


    PlayerHealth playerHealth;
    AudioSource enemyAudio;
    Animator anima;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    Transform player;
    NavMeshAgent nav;
    bool isDead;
    bool isSinking;
    bool playerInRange;
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anima = GetComponent<Animator>();
        hitParticles = GetComponent<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemyAudio = GetComponent<AudioSource>();
        playerHealth = player.GetComponent<PlayerHealth>();
        currentHealth = startingHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange && currentHealth > 0)
        {
            Attack();
        }

        //if (playerHealth.currentHealth <= 0)
        //{
        //    anima.SetTrigger("PlayerDead");
        //}

        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }

        if (currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled = false;
        }
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;
        enemyAudio.Play();
        currentHealth -= amount;
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
        if (currentHealth <= 0)
        {
            Death();
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

    void Death()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
        anima.SetTrigger("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }

    public void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }


}
