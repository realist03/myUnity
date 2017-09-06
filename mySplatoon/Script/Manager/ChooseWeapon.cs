using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWeapon : MonoBehaviour
{
    public PlayerCameraFreeLook playerCamera;

    public ParticleSystem choosPar;

    void Start ()
    {
        Init();
    }

    private void Update()
    {
        if (GameMode.isReady)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.Rotate(0, 3, 0,Space.World);
    }

    public virtual void Init()
    {
        playerCamera = FindObjectOfType<PlayerCameraFreeLook>();
        choosPar = Resources.Load<ParticleSystem>("Prefab/MainVFX");
    }
}
