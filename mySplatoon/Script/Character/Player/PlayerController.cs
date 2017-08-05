using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter player;
    Character character;
    public LineRenderer line;
    public ParticleSystem laserFX;
    float timer;
    float x;
    float y;
    void Start()
    {
        character = GetComponent<Character>();
        character.isBlue = true;
        player = GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            player.Shoot();
            timer = 0;
        }

        if(Input.GetMouseButton(1))
        {
            player.LaserShoot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            laserFX.gameObject.SetActive(true);
            line.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            laserFX.gameObject.SetActive(false);
            line.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.TransColor();
        }

        if(PlayerCharacter.energy == 100 && Input.GetKeyDown(KeyCode.Q))
        {
            player.EnergyShoot();
        }
        if(player.isEnergy)
        {
            player.energyTimer -= Time.deltaTime;
        }
        if(player.energyTimer <= 0.05f)
        {
            player.ExitEnergy();
        }
    }
}
