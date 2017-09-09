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

	void Start ()
    {
        player = GetComponent<PlayerActor>();
    }
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;
        if (GameMode.isReady == false)
            return;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        x = Input.GetAxis("MouseHorizontal");
        y = Input.GetAxis("MouseVertical");

        if (h != 0 || v != 0)
        {
            player.isMove = true;
        }
        else
            player.isMove = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }

        if (Input.GetMouseButton(0))
        {
            player.isFire = true;

            if (player.curWeapon == Actor.eWeapon.Charger)
            {
                player.isCharging = true;
                player.chargingTimer += Time.deltaTime;
            }
        }
        if (!Input.GetMouseButton(0))
        {
            player.isFire = false;

            if (player.curWeapon == Actor.eWeapon.Charger)
            {
                player.isCharging = false;
                player.chargingTimer = 0;
            }
        }

    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        if (player.data.isDie)
            return;
        if (GameMode.isReady == false)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            player.CmdJump();


        if (Input.GetMouseButton(1))
        {
            player.CmdT2Fish();
        }
        if (!Input.GetMouseButton(1))
        {
            player.CmdT2Human();
        }

        player.Move(v, h);
        player.Rotate(y);
        player.RotateWeapon(x);
    }
}
