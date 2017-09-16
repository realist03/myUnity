using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActorController : NetworkBehaviour
{
    PlayerActor player;

    [SyncVar]
    public float h;
    [SyncVar]
    public float v;
    [SyncVar]
    public float x;
    [SyncVar]
    public float y;

    private void Awake()
    {
        player = GetComponent<PlayerActor>();
    }

    void Update()
    {
        if (GameMode.isGameOver)
            return;
        if (!isLocalPlayer)
            return;

        if (player.data.isDie)
            return;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        x = Input.GetAxis("MouseHorizontal");
        y = Input.GetAxis("MouseVertical");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }

 
        if (Input.GetMouseButtonDown(0))
        {
            player.state.isFire = true;

        }

        if (Input.GetMouseButtonUp(0))
        {
            player.state.isFire = false;
             
        }

        if (player.state.isFire)
        {
            if (player.state.curWeapon == Actor.eWeapon.Charger)
            {
                player.state.isCharging = true;
                player.state.chargingTimer += Time.deltaTime;
            }

        }
        else
        {
            if (player.state.curWeapon == Actor.eWeapon.Charger)
            {
                player.state.isCharging = false;
                player.state.chargingTimer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameMode.isGameOver)
            return;

        if (!isLocalPlayer)
            return;
        if (player.data.isDie)
            return;
        if (GameMode.isReady == false)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            player.Jump();


        if (Input.GetMouseButton(1))
        {
            player.Cmd2Fish();
        }
        if (!Input.GetMouseButton(1))
        {
            player.Cmd2Human();
        }

        player.Move(v, h);
        player.Rotate(y);
        //player.RotateWeapon(x);
    }
}
