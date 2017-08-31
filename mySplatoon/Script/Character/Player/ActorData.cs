using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
 
public class ActorData : NetworkBehaviour
{
    public int TeamID;
    public int ID;

    //public Renderer humanModel;
    //public Renderer inkFishModel;
    //public Material blue;
    //public Material red;

    //public ParticleSystem redVFX;
    //public ParticleSystem blueVFX;
    //public ParticleSystem dieVFX;

    //public Transform muzzle;
    public float moveSpeed;
    public float turnSpeed;
    public float jumpHeight;

    public float rushSpeed;
    public float rushJumpHeight;

    public float difSpeed;

    [HideInInspector]
    public float shootTimer;
    public float shootBlank;

    public int reSpawnTime;

    [SyncVar]
    public float health;

    [SyncVar]
    public float ink = 100;

    [SyncVar]
    public int playerShellDamage;

    [SyncVar]
    public float spawnTimer;

    public bool isInkFish = false;
    public bool isDifferent = false;
    public bool isSame = false;
    public bool isReInk = false;
    public bool isInkLow = false;

    [SyncVar]
    public bool isDie = false;

    float damageBlank = 1;

    float damageTImer;

    public float healthMax = 100;
    public float inkMax = 100;
}
	
