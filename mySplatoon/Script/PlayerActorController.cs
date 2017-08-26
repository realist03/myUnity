using System.Collections;
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

        player.Move(v, h);
        player.Rotate(y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }

        if (Input.GetMouseButton(0))
        {
            player.Shoot();
        }

        if (Input.GetKey(KeyCode.E))
        {
            player.TransToInkFish();
        }
        if (Input.GetKeyUp(KeyCode.E))
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
    }
}
