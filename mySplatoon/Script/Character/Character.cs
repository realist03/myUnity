using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class Character : MonoBehaviour
{
    int floorMask;
    float camRayLength;
    bool isDead;

    private Rigidbody rigid;
    private float speed = 4f;
    private float turnSpeed = 120;
    private Vector3 moveMent;
    private Animator anima;
    private int health;
    public float fireForce = 1000;

    public Rigidbody shell;
    public Transform gun;
    public ParticleSystem fireFX;

    public AudioSource step;
    public AudioSource shootAudio;

    public bool isBlue;

    float shootTimer;
    public float shootSpace = 0.2f;
    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        rigid = GetComponent<Rigidbody>();
        anima = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        shootTimer += Time.deltaTime;

        if (!Input.anyKey && step.isPlaying) 
        {
            step.Stop();                    
        }
    }


    public void Move(float h, float v)
    {
        transform.position += transform.forward * v * speed * Time.deltaTime;
        transform.position += transform.right * h * speed * Time.deltaTime;
        if(!step.isPlaying)
        {
            step.Play();
        }
    }

    public void Rotate(float x,float y)
    {
        var xAngles = x * turnSpeed * Time.deltaTime;
        transform.Rotate(0, xAngles, 0);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Death()
    {
        isDead = true;
        anima.SetTrigger("Die");

    }

    public void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anima.SetBool("IsWalking", walking);
    }

    public void Shoot()
    {
        if (shootTimer >= shootSpace)
        {
            Rigidbody ball;
            ParticleSystem shoot;
            ball = Instantiate(shell, gun.position, gun.rotation);
            shoot = Instantiate(fireFX, gun.position, gun.rotation);
            ball.gameObject.SetActive(true);
            shoot.gameObject.SetActive(true);
            fireFX.gameObject.SetActive(true);
            ball.AddForce(transform.forward * fireForce);
            shootAudio.Play();
            Destroy(ball.gameObject, 2);
            Destroy(shoot.gameObject, 2);
            shootTimer = 0;
        }
        else
            return;
    }

}
