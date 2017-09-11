using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slosher : ChooseWeapon
{

    void Start()
    {
        Init();
    }

    private void OnMouseDown()
    {
        GameMode.isReady = true;
        GameMode.tempWeaponID = 1;
        playerCamera.m_LockCursor = true;
        ParticleSystem cp;
        cp = Instantiate(choosPar);
        cp.transform.position = transform.position;
        Destroy(cp.gameObject, 2);
    }
}
