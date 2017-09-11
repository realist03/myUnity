using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : ChooseWeapon
{
    void Start()
    {
        Init();
    }

    private void OnMouseDown()
    {
        GameMode.isReady = true;
        GameMode.tempWeaponID = 3;
        playerCamera.m_LockCursor = true;
        ParticleSystem cp;
        cp = Instantiate(choosPar);
        cp.transform.position = transform.position;
        Destroy(cp, 2);
    }
}
