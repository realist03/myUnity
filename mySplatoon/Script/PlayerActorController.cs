using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActorController : NetworkBehaviour
{
    PlayerActor player;

	void Start ()
    {
        player = GetComponent<PlayerActor>();
    }
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        var x = Input.GetAxis("MouseHorizontal");
        var y = Input.GetAxis("MouseVertical");

        player.Move(v, h);
        player.Rotate(y);

        player.CmdMove(v, h);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }

        if(Input.GetMouseButton(0))
        {
            player.Shoot();
        }

        if(Input.GetKey(KeyCode.E))
        {
            player.TransToInkFish();
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            player.TransToHuman();
        }
    }
}
