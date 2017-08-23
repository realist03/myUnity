using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter player;
	void Start ()
    {
        player = GetComponent<PlayerCharacter>();
	}
	
	void FixedUpdate ()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        var x = Input.GetAxis("MouseHorizontal");
        var y = Input.GetAxis("MouseVertical");

        player.Move(h, v);
        player.Rotate(y);

        if(Input.GetMouseButton(0))
        {
            player.Shoot();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            player.TransColor();
        }

        if(Input.GetKey(KeyCode.Q))
        {
            player.TransToInkFish();
        }
        if(Input.GetKeyUp(KeyCode.Q))
        {
            player.TranToHuman();
        }
	}
}
