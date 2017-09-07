using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActorController : NetworkBehaviour
{
    PlayerActor player;

    public float h;
    public float v;
    public float x;
    public float y;

	void Start ()
    {
        player = GetComponent<PlayerActor>();
    }
	
	void Update ()
    {
        if (!isLocalPlayer)
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

        //if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        //{
        //    player.Shoot();

        //    player.curState = Actor.eState.Fire;
        //    if(player.curWeapon == Actor.eWeapon.Charger)
        //    {
        //        player.isCharging = true;
        //        player.chargingTimer += Time.deltaTime;
        //    }
        //}
        //if(Input.GetMouseButtonUp(0))
        //{
        //    player.curState = Actor.eState.None;

        //    if (player.curWeapon == Actor.eWeapon.Charger)
        //    {
        //        player.isCharging = false;
        //        player.chargingTimer = 0;
        //    }
        //}
        if (Input.GetMouseButton(1))
        {
            player.TransToInkFish();
        }
        if (Input.GetMouseButtonUp(1))
        {
            player.TransToHuman();
        }


    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
            player.CmdJump();

        if (Input.GetMouseButtonDown(0))
        {
            player.CmdShoot();
            player.isFire = true;
        }
        if(Input.GetMouseButton(0))
        {
            player.CmdShoot();
            player.isFire = true;

            if (player.curWeapon == Actor.eWeapon.Charger)
            {
                player.isCharging = true;
                player.chargingTimer += Time.deltaTime;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            player.isFire = false;

            if (player.curWeapon == Actor.eWeapon.Charger)
            {
                player.isCharging = false;
                player.chargingTimer = 0;
            }
        }

        if (Input.GetMouseButton(1))
        {
            player.CmdT2Fish();
        }
        if (Input.GetMouseButtonUp(1))
        {
            player.CmdT2Human();
        }

        player.Move(v, h);
        player.Rotate(y);
    }
}
