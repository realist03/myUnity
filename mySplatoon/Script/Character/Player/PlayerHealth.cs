using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float flashSpeed = 5f;
    public int reCount = 1;
    public bool damaged;

    public AudioClip deathClip;

    public Image postHUD1;
    public Image postHUD2;
    public Image postHUD3;
    public Image postHUD4;
    public Image postHUD5;


    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    public SpriteRenderer post;

    float reTimer;

    bool isDead;

    private PlayerCharacter playerCharacter;
    private ZomBear zomBear;
    private Post shellPost;

    CameraShake shake;
    Animator anim;
    AudioSource playerAudio;

    Color blue = Color.HSVToRGB(0.52f, 0.674f, 1);
    Color red = Color.HSVToRGB(0, 0.368f, 1);


    void Awake()
    {
        playerCharacter = FindObjectOfType<PlayerCharacter>();
        shake = FindObjectOfType<CameraShake>();
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        zomBear = GetComponent<ZomBear>();
        shellPost = GetComponent<Post>();

        currentHealth = startingHealth;
    }

    private void Update()
    {
        reTimer += Time.deltaTime;
        Regeneration();
        if(PlayerCharacter.energy >= 100)
        {
            PlayerCharacter.energy = 100;
        }
        PlayerData.PlayerDataValue.health = currentHealth;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;

        if (currentHealth >= 0)
        {
            currentHealth -= amount;
        }


        playerAudio.Play();

        shake.PlayerUnderAttackShake();
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        anim.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

    }

    private void OnCollisionEnter(Collision collision)
    {
        var temp = collision.gameObject.GetComponent<Post>();

        if (collision.gameObject.tag == "Shell")
        {
            if (playerCharacter.isBlue && !temp.isPostBlue)
            {
                TakeDamage(10);
            }
            else if (playerCharacter.isBlue && temp.isPostBlue)
            {
                PlayerCharacter.energy += 5;
            }
            else if (!playerCharacter.isBlue && temp.isPostBlue)
            {
                TakeDamage(10);
            }
            else if (!playerCharacter.isBlue && !temp.isPostBlue)
            {
                PlayerCharacter.energy += 5;
            }
        }
        else
        {
            return;
        }
        if (currentHealth <= 90)
        {
            postHUD1.gameObject.SetActive(true);
            if (temp.isPostBlue)
            {
                postHUD1.color = blue;
            }
            else
                postHUD1.color = red;
        }
        if (currentHealth <= 70)
        {
            postHUD2.gameObject.SetActive(true);
            if (temp.isPostBlue)
            {
                postHUD2.color = blue;
            }
            else
                postHUD2.color = red;
        }
        if (currentHealth <= 60)
        {
            postHUD3.gameObject.SetActive(true);
            if (temp.isPostBlue)
            {
                postHUD3.color = blue;
            }
            else
                postHUD3.color = red;
        }
        if (currentHealth <= 40)
        {
            postHUD4.gameObject.SetActive(true);
            if (temp.isPostBlue)
            {
                postHUD4.color = blue;
            }
            else
                postHUD4.color = red;
        }
        if (currentHealth <= 30)
        {
            postHUD5.gameObject.SetActive(true);
            if (temp.isPostBlue)
            {
                postHUD5.color = blue;
            }
            else
                postHUD5.color = red;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Regeneration()
    {
        if (reTimer >= 1)
        {
            if (currentHealth <= 100)
            {
                currentHealth += reCount;
                reTimer = 0;
            }
        }
    }
}
