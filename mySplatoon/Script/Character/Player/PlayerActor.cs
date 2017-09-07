using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActor : Actor
{
    PlayerCamera newCamera;
    bool isInkLow = false;
    bool isReInk = false;

    protected override void Init()
    {
        base.Init();
        newCamera = FindObjectOfType<PlayerCamera>();
        if (isLocalPlayer)
        {
            newCamera.character = transform;
        }
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
    [Command]
    public void CmdT2Fish()
    {
        RpcT2Fish();
    }
    [ClientRpc]
    public void RpcT2Fish()
    {
        if (curFish == eInkFish.Human)
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
    [Command]
    public void CmdT2Human()
    {
        RpcT2Human();
    }
    [ClientRpc]
    public void RpcT2Human()
    {
        if (curFish == eInkFish.InkFish)
        {
            curFish = eInkFish.Human;
            model.HumanModel.SetActive(true);
            model.InkFishModel.SetActive(false);
        }
    }
}
