using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter player;

    float h;
    float v;
    float x;
    float y;

	void Start ()
    {
        player = GetComponent<PlayerCharacter>();
	}
    private void FixedUpdate()
    {
        player.Move(h, v);
        player.Rotate(y);
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        x = Input.GetAxis("MouseHorizontal");
        y = Input.GetAxis("MouseVertical");


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
            if (player.isInkFish && player.isSame)
            {
                player.isReInk = true;
            }
            else
            {
                player.isReInk = false;
            }

        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            player.TranToHuman();
            player.isReInk = false;
        }

        if (Input.GetKey(KeyCode.Space)) player.jumpInput = true;
        else player.jumpInput = false;

    }
}
