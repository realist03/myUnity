using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public Renderer playerModel;

    void Start ()
    {
		
	}

    protected override void Update()
    {
        base.Update();
    }

    public void LaserShoot()
    {

    }

    public void TransColor()
    {
        if(curColor == chaColor.Blue)
        {
            curColor = chaColor.Red;
            playerModel.sharedMaterial = red;
            shellM.sharedMaterial = red;
        }
        else
        {
            curColor = chaColor.Blue;
            playerModel.sharedMaterial = blue;
            shellM.sharedMaterial = blue;
        }
    }

    protected override void Die()
    {
        base.Die();
        GameMode.isGameOver = true;
    }
}
