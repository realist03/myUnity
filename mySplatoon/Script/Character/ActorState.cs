using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class ActorState : Actor
{

    public bool isChoose = false;
    public bool isFire = false;
    public bool isMove = false;
    public bool isCharging = false;
    public bool isPosting = false;

    public eInkFish curFish = eInkFish.Human;
    public eColor curColor = eColor.None;
    public eState curState = eState.None;
    public eSame curSame = eSame.None;
    public eWeapon curWeapon;
    public float chargingTimer;

    void Update()
    {
        CmdActorState(isChoose, isFire, isMove, isCharging);
        CmdActorEnum(curFish, curColor, curState, curSame, curWeapon);

        if (GameMode.isGameOver == true)
            return;

        if (!GameMode.isReady)
            return;

        if(data.isDie == true)
        {
            data.spawnTimer += Time.deltaTime;
        }

        if (data.spawnTimer >= data.reSpawnTime)
            Respawn(self);

        data.shootTimer += Time.deltaTime;

        curWeapon = (eWeapon)GameMode.tempWeaponID;
        CmdWeapon(netId.Value, curWeapon);

        CheckAnim();
        CheckMapColor();
        CheckCurJumpState();
        CheckIsInkLow();
        CheckIsReInk();
        CheckPainted();
        CheckRunVFX();
        CheckIsDie();
        CheckInkFish();

        if (controller.h != 0 || controller.v != 0)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }

    private void FixedUpdate()
    {
        CmdTeamId(netId.Value, data.TeamID);

        if (isFire == true)
        {
            Shoot();
        }


        RegenerateInk();
        RegenerateHealth();
    }

    [Command]
    public void CmdActorState(bool r1, bool r2, bool r3, bool r4)
    {
        RpcCheckBoolState(r1, r2, r3, r4);
    }
    [ClientRpc]
    public void RpcCheckBoolState(bool r1, bool r2, bool r3, bool r4)
    {
        if (!isLocalPlayer)
        {
            isChoose = r1;
            isFire = r2;
            isMove = r3;
            isCharging = r4;
        }
    }

    [Command]
    public void CmdActorEnum(eInkFish e1, eColor e2, eState e3, eSame e4, eWeapon e5)
    {
        RpcActorEnum(e1, e2, e3, e4, e5);
    }
    [ClientRpc]
    public void RpcActorEnum(eInkFish e1, eColor e2, eState e3, eSame e4, eWeapon e5)
    {
        if(!isLocalPlayer)
        {
            curFish = e1;
            curColor = e2;
            curState = e3;
            curSame = e4;
            curWeapon = e5;
        }
    }
}
