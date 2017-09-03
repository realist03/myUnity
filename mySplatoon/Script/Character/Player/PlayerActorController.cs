﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActorController : NetworkBehaviour
{
    PlayerActor player;

    float h;
    float v;
    float x;
    float y;

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

        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            player.Shoot();
        }

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

        if (Input.GetMouseButton(0))
            player.CmdShoot();

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
