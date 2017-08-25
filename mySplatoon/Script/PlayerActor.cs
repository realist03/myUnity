﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    protected override void Init()
    {
        base.Init();
	}
	
	protected override void Update ()
    {
        base.Update();
	}

    public void TransToInkFish()
    {
        if(curFish == eInkFish.Human)
        {
            curFish = eInkFish.InkFish;
            model.HumanModel.SetActive(false);
            model.InkFishModel.SetActive(true);
        }
    }

    public void TransToHuman()
    {
        if (curFish == eInkFish.InkFish)
        {
            curFish = eInkFish.Human;
            model.HumanModel.SetActive(true);
            model.InkFishModel.SetActive(false);
        }
    }
}