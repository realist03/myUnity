﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public Renderer playerModel;
    public Renderer InkFish;

    protected override void Update()
    {
        base.Update();
    }

    public void TransColor()
    {
        if (curColor == chaColor.Blue)
        {
            curColor = chaColor.Red;
            playerModel.sharedMaterial = red;
            InkFish.sharedMaterial = red;
        }
        else
        {
            curColor = chaColor.Blue;
            playerModel.sharedMaterial = blue;
            InkFish.sharedMaterial = blue;
        }
    }

    protected override void Die()
    {
        base.Die();
        GameMode.isGameOver = true;
    }
}
