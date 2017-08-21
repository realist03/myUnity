using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum chaColor
    {
        None,
        Red,
        Blue,
    }


    public chaColor curColor = chaColor.None;


    public Renderer humanModel;
    public Renderer inkFishModel;
    public Renderer shellM;
    public Material blue;
    public Material red;

    public Rigidbody shell;
    public Transform muzzle;
    public float moveSpeed;
    public float turnSpeed;
    public int health;
    public int fireForce;
    public int ink = 100;
    public int playerShellDamage;

    public bool isInkFish = false;

    float shootBlank = 0.2f;
    float shootTimer;
    bool canShoot;

    Animator animator;
	void Start ()
    {
        animator = GetComponent<Animator>();
    }
	
	protected virtual void Update ()
    {
        if (GameMode.isGameOver == true)
            return;
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootBlank)
        {
            canShoot = true;
        }

        if (health <= 0)
        {
            Die();
        }
        CheckMapColor();
	}

    public void Move(float h, float v)
    {
        transform.position += transform.forward * v * moveSpeed * Time.deltaTime;
        transform.position += transform.right * h * moveSpeed * Time.deltaTime;
    }

    public void Rotate(float v)
    {
        var Angles = -v * turnSpeed * Time.deltaTime;
        transform.Rotate(0, Angles, 0);
    }

    public void Shoot()
    {
        if (canShoot == false || ink <= 0)
        {
            return;
        }
        canShoot = false;
        shootTimer = 0;

        ink -= 10;
        Rigidbody shell_;
        shell_ = Instantiate(shell, muzzle.position, muzzle.rotation) as Rigidbody;
        shell_.gameObject.SetActive(true);
        shell_.AddForce(transform.forward * fireForce);
        Destroy(shell_.gameObject, 3);

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    protected virtual void Die()
    {

    }


    public void TransToInkFish()
    {
        if(isInkFish == false)
        {
            isInkFish = true;
            //animator.Set
            humanModel.gameObject.SetActive(false);
            inkFishModel.gameObject.SetActive(true);
        }
        else
        {
            isInkFish = false;
            //animator.Set
            humanModel.gameObject.SetActive(true);
            inkFishModel.gameObject.SetActive(false);
        }
    }

    public void CheckPositon()
    {
        //if (transform.position && isInkFish = true;)
        //{
        //    RegenerateInk();
        //}
    }

    public void RegenerateInk()
    {
        ink += 20;
    }

    public bool CheckMapColor()
    {
        var posX = Mathf.FloorToInt(transform.position.x);
        var posY = Mathf.FloorToInt(transform.position.z);
        //Debug.Log(posX + "," + posY);
        chaColor checkColor = chaColor.None;

        if(Mapping.painted.ContainsKey(new Vector2(posX,posY)))
        {
            Debug.Log("have");
            if (Mapping.painted.TryGetValue(new Vector2(posX, posY), out checkColor))
            {
                if (checkColor != chaColor.None && checkColor != curColor)
                {
                    Debug.Log("dif");
                    return false;
                }
                if(checkColor != chaColor.None && checkColor == curColor)
                {
                    Debug.Log("same");
                    return true;
                }
            }
        }
        return true;
    }
} 
