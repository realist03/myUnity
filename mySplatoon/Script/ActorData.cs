using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ActorData : MonoBehaviour
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

    //public float health;

    //public float ink = 100;
    //public int playerShellDamage;

    //public bool isInkFish = false;
    //public bool isDifferent = false;
    //public bool isSame = false;
    //public bool isReInk = false;

    //float shootBlank = 0.2f;
    //float damageBlank = 1;

    //float shootTimer;
    //float damageTImer;

    //bool canShoot;

    //HttpUser httpUser;
    //Animator animator;

}
	
