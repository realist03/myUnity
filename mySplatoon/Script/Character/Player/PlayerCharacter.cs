using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public static int energy;
    public float energyTimer = 0;
    public bool isEnergy = false;

    public Material redPlayer;
    public Material bluePlayer;

    public Material redShellM;
    public Material blueShellM;

    public Renderer playerRenderer;
    public Renderer shellRenderer;
    public SpriteRenderer post;

    public ParticleSystem redFire;
    public ParticleSystem blueFire;

    public ParticleSystem shellFX;

    public Transform muzzle;
    public LineRenderer line;

    public AudioSource laser;

    Ray ray;
    RaycastHit hit;

    Color blue = Color.HSVToRGB(0.52f, 0.674f, 1);
    Color red = Color.HSVToRGB(0, 0.368f, 1);
    Character character;

    void Start()
    {
        character = GetComponent<Character>();
        line.SetVertexCount(2);//设置顶点
    }

    protected override void Update()
    {
        if (!Input.anyKey && step.isPlaying)
        {
            laser.Stop();
        }
        PlayerData.PlayerDataValue.Power = energy;
        base.Update();
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        Move(h, v);
        Rotate(x, y);
        Animating(h, v);
    }

    public void TransColor()
    {
        if (isBlue)
        {
            isBlue = false;

            character.fireFX = redFire;
            playerRenderer.material = redPlayer;
            shellRenderer.material = redShellM;
            post.color = red;
        }
        else
        {
            isBlue = true;

            character.fireFX = blueFire;
            playerRenderer.material = bluePlayer;
            shellRenderer.material = blueShellM;
            post.color = blue;
        }
    }

    public void LaserShoot()
    {
        ray = new Ray(transform.position, transform.forward);
        ray.origin = new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z);
        ray.direction = transform.forward;

        line.SetPosition(0, ray.origin);//设置开始坐标

        if (Physics.Raycast(ray, out hit, 100))
        {
            var enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1, hit.point);
            }

            line.SetPosition(1, hit.point);
        }
        else
        {
            line.SetPosition(1, ray.origin + ray.direction * 100);
        }
        if (!laser.isPlaying)
        {
            laser.Play();
        }
    }

    public void EnergyShoot()
    {
        isEnergy = true;
        energy =0;
        energyTimer = 10;
        shootSpace = 0;
    }

    public void ExitEnergy()
    {
        isEnergy = false;
        energyTimer = 0;
        shootSpace = 0.2f;
    }
}
