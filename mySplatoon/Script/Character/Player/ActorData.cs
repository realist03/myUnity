using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
 
public class ActorData : NetworkBehaviour
{
    [SyncVar] public float health;
    [SyncVar] public float ink = 100;
    [SyncVar] public int playerShellDamage;
    [SyncVar] public float spawnTimer;
    [SyncVar] public bool isReInk = false;
    [SyncVar] public bool isInkFish = false;
    [SyncVar] public bool isInkLow = false;
    [SyncVar] public bool isDie = false;

    [SyncVar] public float moveSpeed;
    [SyncVar] public float turnSpeed;
    [SyncVar] public float jumpHeight;

    [SyncVar] public int points = 0;
    [SyncVar] public int power = 0;
    public int powerMax = 100;

    [HideInInspector] [SyncVar] public float shootTimer;

    public int TeamID;
    public int ID;
    public int weaponID;

    public float normalSpeed;
    public float normalHeight;

    public float rushSpeed;
    public float rushJumpHeight;

    public float difSpeed;

    public float splatterBlank;
    public float rollerBlank;
    public float shootCost;
    public float rollerCost;
    public int reSpawnTime;


    public float healthMax = 100;

    public float inkMax = 100;
}
	
