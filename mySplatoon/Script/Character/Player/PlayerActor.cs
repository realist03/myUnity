using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActor : Actor
{
    PlayerCamera newCamera;

    protected override void Init()
    {
        base.Init();
        newCamera = FindObjectOfType<PlayerCamera>();
        if (isLocalPlayer)
        {
            newCamera.character = transform;
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
            AddTransFX();
            model.humanModel.SetActive(false);
            model.inkFishModel.SetActive(true);
            animator.SetBool("Trans",true);
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
            AddTransFX();
            model.humanModel.SetActive(true);
            model.inkFishModel.SetActive(false);
            animator.SetBool("Trans", false);
        }
    }
}
