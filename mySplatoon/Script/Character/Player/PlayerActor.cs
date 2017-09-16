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
	
    public void TransToFish()
    {
        if (state.curFish == eInkFish.Human)
        {
            state.curFish = eInkFish.InkFish;
            AddTransFX();
            model.humanModel.SetActive(false);
            model.inkFishModel.SetActive(true);
            animator.SetBool("Trans", true);
        }
    }
    [Command]
    public void Cmd2Fish()
    {
        Rpc2Fish();
    }
    [ClientRpc]
    public void Rpc2Fish()
    {
        if (state.curFish == eInkFish.Human && state.isFire == false)
        {
            state.curFish = eInkFish.InkFish;
            AddTransFX();
            model.humanModel.SetActive(false);
            model.inkFishModel.SetActive(true);
            animator.SetBool("Trans", true);
        }
    }


    public void TransToHuman()
    {
        if (state.curFish == eInkFish.InkFish)
        {
            state.curFish = eInkFish.Human;
            AddTransFX();
            model.humanModel.SetActive(true);
            model.inkFishModel.SetActive(false);
            animator.SetBool("Trans", false);
            animator.SetTrigger("TransB");
        }
    }
    [Command]
    public void Cmd2Human()
    {
        Rpc2Huam();
    }
    [ClientRpc]
    public void Rpc2Huam()
    {
        if (state.curFish == eInkFish.InkFish)
        {
            state.curFish = eInkFish.Human;
            AddTransFX();
            model.humanModel.SetActive(true);
            model.inkFishModel.SetActive(false);
            animator.SetBool("Trans", false);
            animator.SetTrigger("TransB");
        }
    }
}
