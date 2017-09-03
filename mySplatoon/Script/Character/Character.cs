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
    public Material blue;
    public Material red;

    public ParticleSystem redVFX;
    public ParticleSystem blueVFX;
    public ParticleSystem dieVFX;

    public Transform muzzle;

    public float moveSpeed;
    public float turnSpeed;
    public float health;

    public float ink = 100;
    public int playerShellDamage;

    public bool isInkFish = false;
    public bool isDifferent = false;
    public bool isSame = false;
    public bool isReInk = false;

    float shootBlank = 0.2f;
    float damageBlank = 1;

    float shootTimer;
    float damageTImer;

    bool canShoot;

    HttpUser httpUser;
    Animator animator;
	void Start ()
    {
        httpUser = FindObjectOfType<HttpUser>();

        animator = GetComponent<Animator>();
    }
	
	protected virtual void Update ()
    {
        CheckState();
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
        if (canShoot == false || ink < 10)
        {
            return;
        }
        canShoot = false;
        shootTimer = 0;

        ink -= 5;
        ParticleSystem shootVFX;
        if(curColor == chaColor.Red)
        {
            shootVFX = Instantiate(redVFX, muzzle.position, redVFX.gameObject.transform.rotation, transform) as ParticleSystem;
            shootVFX.gameObject.SetActive(true);
            Destroy(shootVFX, 1);
        }
        else if(curColor == chaColor.Blue)
        {
            shootVFX = Instantiate(blueVFX, muzzle.position, blueVFX.gameObject.transform.rotation, transform) as ParticleSystem;
            shootVFX.gameObject.SetActive(true);
            Destroy(shootVFX, 1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (gameObject.tag == "Player")
        {
            httpUser.SendSave();
            Debug.Log("掉血啦");
        }

        health -= damage;
    }

    protected virtual void Die()
    {
        ParticleSystem die;
        die = Instantiate(dieVFX, transform.localPosition, transform.localRotation, transform);
        die.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Destroy(gameObject,2);
    }


    public void TransToInkFish()
    {
        isInkFish = true;
        //animator.Set
        humanModel.gameObject.SetActive(false);
        inkFishModel.gameObject.SetActive(true);

    }

    public void TranToHuman()
    {
        isInkFish = false;
        //animator.Set
        humanModel.gameObject.SetActive(true);
        inkFishModel.gameObject.SetActive(false);
    }

    public void CheckPositon()
    {
        //if (transform.position && isInkFish = true;)
        //{
        //    RegenerateInk();
        //}
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
                    isDifferent = true;
                    isSame = false;
                    Debug.Log("dif");
                    return false;
                }
                if(checkColor != chaColor.None && checkColor == curColor)
                {
                    isDifferent = false;
                    isSame = true;
                    Debug.Log("same");
                    return true;
                }
            }
        }
        isDifferent = false;
        isSame = false;
        return true;
    }

    public void CheckState()
    {
        if (health <= 0)
        {
            Die();
        }
        if(ink < 0)
        {
            ink = 0;
        }
        if(ink > 100)
        {
            ink = 100;
        }
        if (health < 0)
        {
            health = 0;
        }
        if (health > 100)
        {
            health = 100;
        }

        if (ink < 100)
        {
            ink += 0.03f;
        }

        if(health < 100)
        {
            health += 0.03f;
        }

        if (GameMode.isGameOver == true)
            return;

        shootTimer += Time.deltaTime;
        damageTImer += Time.deltaTime;


        if (isInkFish)
        {
            CheckMapColor();

            if (isDifferent)
            {
                moveSpeed = 3;

                if (damageTImer >= damageBlank)
                {
                    TakeDamage(10);
                    damageTImer = 0;
                }
            }
            else
            {
                if (isInkFish && isSame)
                {
                    if(ink <= 100)
                    {
                        ink += 1;
                    }
                    if(health < 100)
                    {
                        health += 0.3f;
                    }
                    moveSpeed = 10;
                }
                else
                {
                    moveSpeed = 6;
                }
            }
            if (isSame == false)
            {
                moveSpeed = 3;
            }
        }
        else
        {
            CheckMapColor();

            moveSpeed = 6;

            if (shootTimer >= shootBlank)
            {
                canShoot = true;
            }

            if (isDifferent)
            {
                if (damageTImer >= damageBlank)
                {
                    TakeDamage(10);
                    damageTImer = 0;
                }
                moveSpeed = 3;
            }
        }

    }

    public void InitMaterial()
    {
        if (isInkFish)
        {
            if (curColor == chaColor.Blue)
            {
                humanModel.sharedMaterial = blue;
            }
            else
            {
                humanModel.sharedMaterial = red;
            }
        }
        else
        {
            if (curColor == chaColor.Blue)
            {
                inkFishModel.sharedMaterial = blue;
            }
            else
            {
                inkFishModel.sharedMaterial = red;
            }
        }
    }

}
