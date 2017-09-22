using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEngine.UI;

public class Actor : NetworkBehaviour
{
    [HideInInspector] public ActorData data;
    [HideInInspector] public ActorModel model;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerActorController controller;
    [HideInInspector] public ActorState state;
    [HideInInspector] public GameObject self;

    [SerializeField] Image fillImage;
    [SerializeField] Image powerFillImage;

    [SerializeField] AudioSource shootAudio;

    public LayerMask layer;

    Vector3 spawn;

    Rigidbody rigid;
    PlayerActor playerActor;
    CameraShake shake;

    PlayerCameraFreeLook camera;
    ScreenPost screenPost;
    ChooseWeapon choose;

    Transform screenPoint;
    private Renderer render;

    public enum eColor
    {
        None=0,

        One_Purple=1,
        One_WarmYellow=2,

        Two_LightBlue,
        Two_ColdYellow,

        Three_Green_Blue,
        Three_Orange,

        Four_Green_Yellow,
        Four_Red_Purple,
    }

    public enum eState
    {
        None,
        Jump,
    }

    public enum eInkFish
    {
        InkFish,
        Human,
    }

    public enum eSame
    {
        None,
        Same,
        Diffent,
    }

    public enum eWeapon
    {
        Splattershot,
        Slosher,
        Charger,
        Roller,
    }



    public void Awake()
    {
        controller = GetComponent<PlayerActorController>();
        data = GetComponent<ActorData>();
        state = GetComponent<ActorState>();
        model = GetComponent<ActorModel>();
        rigid = GetComponent<Rigidbody>();
        choose = FindObjectOfType<ChooseWeapon>();
        animator = GetComponentInChildren<Animator>();
        shake = FindObjectOfType<CameraShake>();
        camera = FindObjectOfType<PlayerCameraFreeLook>();
        screenPost = camera.GetComponentInChildren<ScreenPost>(true);
        playerActor = GetComponent<PlayerActor>();
        spawn = gameObject.transform.position;
        data.health = data.healthMax;
        self = gameObject;
    }

    public void Start()
    {
        gameObject.name = "Player" + (netId.Value-1);

        if(isLocalPlayer)
        {
            Util.DelayCall(3, () =>
            {
                Init();
                CmdTeamId(netId.Value, data.TeamID);
            });
        }

    }

    protected virtual void Init()
    {
        if (isLocalPlayer)
        {
            data.TeamID = BattleManager.Instance.curPlayerTeam;
        }
    }

    [Command]
    public void CmdTeamId(uint rNetId, int value)
    {
        RpcGetTeamId(rNetId, value);
    }

    [ClientRpc]
    public void RpcGetTeamId(uint rNetId, int value)
    {
        if (netId.Value == rNetId)
        {
            data.TeamID = value;
            InitCurColor();
        }
    }
    [Command]
    public void CmdWeapon(uint rNetId,eWeapon cWeapon)
    {
        RpcWeapon(rNetId, cWeapon);
    }
    [ClientRpc]
    public void RpcWeapon(uint rNetId, eWeapon cWeapon)
    {
        if (netId.Value == rNetId)
        {
            InitWeapon(cWeapon);
        }
    }

    public void InitCurColor()
    {
        if (BattleManager.Instance.curColorPair == 1)
        {
            if(data.TeamID == 0)
            {
                state.curColor = eColor.One_Purple;
            }
            else
            {
                state.curColor = eColor.One_WarmYellow;
            }
        }
        else if(BattleManager.Instance.curColorPair == 2)
        {
            if (data.TeamID == 0)
            {
                state.curColor = eColor.Two_LightBlue;
            }
            else
            {
                state.curColor = eColor.Two_ColdYellow;
            }
        }
        else if (BattleManager.Instance.curColorPair == 3)
        {
            if (data.TeamID == 0)
            {
                state.curColor = eColor.Three_Green_Blue;
            }
            else
            {
                state.curColor = eColor.Three_Orange;
            }
        }
        else if (BattleManager.Instance.curColorPair == 4)
        {
            if (data.TeamID == 0)
            {
                state.curColor = eColor.Four_Green_Yellow;
            }
            else
            {
                state.curColor = eColor.Four_Red_Purple;
            }
        }
        Debug.Log(gameObject.name+"的颜色为"+ state.curColor);

        SetCurColor();
    }

    public void SetCurColor()
    {
        switch (state.curColor)
        {
            case eColor.None:
                break;
            case eColor.One_Purple:
                var ps1 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps1.Length; i++)
                {
                    var  item = ps1[i].main;
                    item.startColor = model.One_Purple;
                    var trail = ps1[i].trails;
                    trail.colorOverTrail = model.One_Purple;
                    var post = ps1[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.One_Purple;
                    }
                    if(ps1[i].name == "ShellVFX3" || ps1[i].name == "ShellVFX3_2" || ps1[i].name == "RollerShellVFX")
                    {
                        var post_m = ps1[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_One_Purple;
                        }
                    }
                }

                fillImage.color = model.One_Purple;
                powerFillImage.color = model.One_Purple;
                model.inkBag.sharedMaterial = model.m_One_Purple;


                var InkFish1 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing1 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish1.materials[0].color = model.One_Purple;

                for (int i = 0; i < clothing1.Length; i++)
                {
                    clothing1[i].materials[0].color = model.One_Purple;
                }

                break;
            case eColor.One_WarmYellow:
                var ps2 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps2.Length; i++)
                {
                    var item = ps2[i].main;
                    item.startColor = model.One_WarmYellow;
                    var trail = ps2[i].trails;
                    trail.colorOverTrail = model.One_WarmYellow;

                    var post = ps2[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.One_WarmYellow;
                    }

                    if (ps2[i].name == "ShellVFX3" || ps2[i].name == "ShellVFX3_2" || ps2[i].name == "RollerShellVFX")
                    {
                        var post_m = ps2[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_One_WarmYellow;
                        }
                    }
                }

                fillImage.color = model.One_WarmYellow;
                powerFillImage.color = model.One_WarmYellow;

                model.inkBag.sharedMaterial = model.m_One_WarmYellow;
                var InkFish2 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing2 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish2.materials[0].color = model.One_WarmYellow;

                for (int i = 0; i < clothing2.Length; i++)
                {
                    clothing2[i].materials[0].color = model.One_WarmYellow;
                }

                break;
            case eColor.Two_LightBlue:
                var ps3 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps3.Length; i++)
                {
                    var item = ps3[i].main;
                    item.startColor = model.Two_LightBlue;
                    var trail = ps3[i].trails;
                    trail.colorOverTrail = model.Two_LightBlue;

                    var post = ps3[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Two_LightBlue;
                    }

                    if (ps3[i].name == "ShellVFX3" || ps3[i].name == "ShellVFX3_2" || ps3[i].name == "RollerShellVFX")
                    {
                        var post_m = ps3[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Two_LightBlue;
                        }
                    }
                }

                fillImage.color = model.Two_LightBlue;
                powerFillImage.color = model.Two_LightBlue;

                model.inkBag.sharedMaterial = model.m_Two_LightBlue;
                var InkFish3 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing3 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish3.materials[0].color = model.Two_LightBlue;

                for (int i = 0; i < clothing3.Length; i++)
                {
                    clothing3[i].materials[0].color = model.Two_LightBlue;
                }

                break;
            case eColor.Two_ColdYellow:
                var ps4 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps4.Length; i++)
                {
                    var item = ps4[i].main;
                    item.startColor = model.Two_ColdYellow;
                    var trail = ps4[i].trails;
                    trail.colorOverTrail = model.Two_ColdYellow;

                    var post = ps4[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Two_ColdYellow;
                    }

                    if (ps4[i].name == "ShellVFX3" || ps4[i].name == "ShellVFX3_2" || ps4[i].name == "RollerShellVFX")
                    {
                        var post_m = ps4[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Two_ColdYellow;
                        }
                    }
                }

                fillImage.color = model.Two_ColdYellow;
                powerFillImage.color = model.Two_ColdYellow;

                model.inkBag.sharedMaterial = model.m_Two_ColdYellow;
                var InkFish4 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing4 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish4.materials[0].color = model.Two_ColdYellow;

                for (int i = 0; i < clothing4.Length; i++)
                {
                    clothing4[i].materials[0].color = model.Two_ColdYellow;
                }

                break;
            case eColor.Three_Green_Blue:
                var ps5 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps5.Length; i++)
                {
                    var item = ps5[i].main;
                    item.startColor = model.Three_Green_Blue;
                    var trail = ps5[i].trails;
                    trail.colorOverTrail = model.Three_Green_Blue;

                    var post = ps5[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Three_Green_Blue;
                    }

                    if (ps5[i].name == "ShellVFX3" || ps5[i].name == "ShellVFX3_2" || ps5[i].name == "RollerShellVFX")
                    {
                        var post_m = ps5[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Three_Green_Blue;
                        }
                    }
                }

                fillImage.color = model.Three_Green_Blue;
                powerFillImage.color = model.Three_Green_Blue;

                model.inkBag.sharedMaterial = model.m_Three_Green_Blue;
                var InkFish5 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing5 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish5.materials[0].color = model.Three_Green_Blue;

                for (int i = 0; i < clothing5.Length; i++)
                {
                    clothing5[i].materials[0].color = model.Three_Green_Blue;
                }

                break;
            case eColor.Three_Orange:
                var ps6 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps6.Length; i++)
                {
                    var item = ps6[i].main;
                    item.startColor = model.Three_Orange;
                    var trail = ps6[i].trails;
                    trail.colorOverTrail = model.Three_Orange;

                    var post = ps6[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Three_Orange;
                    }

                    if (ps6[i].name == "ShellVFX3" || ps6[i].name == "ShellVFX3_2" || ps6[i].name == "RollerShellVFX")
                    {
                        var post_m = ps6[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Three_Orange;
                        }
                    }
                }

                fillImage.color = model.Three_Orange;
                powerFillImage.color = model.Three_Orange;

                model.inkBag.sharedMaterial = model.m_Three_Orange;
                var InkFish6 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing6 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish6.materials[0].color = model.Three_Orange;

                for (int i = 0; i < clothing6.Length; i++)
                {
                    clothing6[i].materials[0].color = model.Three_Orange;
                }

                break;
            case eColor.Four_Green_Yellow:
                var ps7 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps7.Length; i++)
                {
                    var item = ps7[i].main;
                    item.startColor = model.Four_Green_Yellow;
                    var trail = ps7[i].trails;
                    trail.colorOverTrail = model.Four_Green_Yellow;

                    var post = ps7[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Four_Green_Yellow;
                    }

                    if (ps7[i].name == "ShellVFX3" || ps7[i].name == "ShellVFX3_2" || ps7[i].name == "RollerShellVFX")
                    {
                        var post_m = ps7[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Four_Green_Yellow;
                        }
                    }
                }

                fillImage.color = model.Four_Green_Yellow;
                powerFillImage.color = model.Four_Green_Yellow;

                model.inkBag.sharedMaterial = model.m_Four_Green_Yellow;
                var InkFish7 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing7 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish7.materials[0].color = model.Four_Green_Yellow;

                for (int i = 0; i < clothing7.Length; i++)
                {
                    clothing7[i].materials[0].color = model.Four_Green_Yellow;
                }

                break;
            case eColor.Four_Red_Purple:
                var ps8 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps8.Length; i++)
                {
                    var item = ps8[i].main;
                    item.startColor = model.Four_Red_Purple;
                    var trail = ps8[i].trails;
                    trail.colorOverTrail = model.Four_Red_Purple;

                    var post = ps8[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Four_Red_Purple;
                    }

                    if (ps8[i].name == "ShellVFX3" || ps8[i].name == "ShellVFX3_2" || ps8[i].name == "RollerShellVFX")
                    {
                        var post_m = ps8[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Four_Red_Purple;
                        }
                    }
                }

                fillImage.color = model.Four_Red_Purple;
                powerFillImage.color = model.Four_Red_Purple;

                model.inkBag.sharedMaterial = model.m_Four_Red_Purple;
                var InkFish8 = model.inkFishModel.GetComponentInChildren<Renderer>(true);
                var clothing8 = model.clothing.GetComponentsInChildren<Renderer>();

                InkFish8.materials[0].color = model.Four_Red_Purple;

                for (int i = 0; i < clothing8.Length; i++)
                {
                    clothing8[i].materials[0].color = model.Four_Red_Purple;
                }

                break;
            default:
                break;
        }
    }

    public void InitWeapon(eWeapon curWeapon)
    {
 
        if (state.isChoose == true)
            return;
        switch (curWeapon)
        {
            case eWeapon.Splattershot:
                Instantiate(Resources.Load<GameObject>("Prefab/Weapon/Weapon-Splattershot"),model.weapon.position,Quaternion.Euler(0,180,0),model.weapon);
                break;
            case eWeapon.Slosher:
                Instantiate(Resources.Load<GameObject>("Prefab/Weapon/Weapon-Slosher"), model.weapon.position, Quaternion.Euler(0, 180, 0), model.weapon);
                break;
            case eWeapon.Charger:
                Instantiate(Resources.Load<GameObject>("Prefab/Weapon/Weapon-Charger"), model.weapon.position, Quaternion.Euler(0, 180, 0), model.weapon);
                break;
            case eWeapon.Roller:
                Instantiate(Resources.Load<GameObject>("Prefab/Weapon/Weapon-Roller"), model.weapon);
                data.normalSpeed = 2;
                break;
            default:
                break;
        }
        state.isChoose = true;
    }
    public void CheckMapColor()
    {
        if (state.curState == eState.Jump)
            return;
        var posX = Mathf.FloorToInt(transform.position.x);
        var posY = Mathf.FloorToInt(transform.position.z);

        eColor mapColor;

        if (Mapping.map.ContainsKey(new Vector2(posX, posY)))
        {
            if (Mapping.map.TryGetValue(new Vector2(posX, posY), out mapColor))
            {
                if (mapColor != eColor.None && mapColor != state.curColor)
                {
                    state.curSame = eSame.Diffent;
                    TakeMapDamage(10);

                    DifEffect();
                }
                else
                    NormalEffect();

                if (mapColor != eColor.None && mapColor == state.curColor)
                {
                    state.curSame = eSame.Same;
                }
            }
        }
        else
            state.curSame = eSame.None;
    }

    public void CheckPainted()
    {
        if (state.curFish == eInkFish.InkFish && state.curSame == eSame.Same)
        {
            data.moveSpeed = data.rushSpeed;
            data.jumpHeight = data.rushJumpHeight;
        }
        else
        {
            data.moveSpeed = data.normalSpeed;
            data.jumpHeight = data.normalHeight; ;
        }

        if (state.curFish == eInkFish.InkFish && state.curSame != eSame.Same)
        {
            data.moveSpeed = data.difSpeed;
        }
    }

    public void CheckCurJumpState()
    {
        if (Physics.Raycast(transform.position, -transform.up, 0.2f))
        {
            if(state.curState == eState.Jump)
            {
                AddFloorPost(transform.position);
            }
            state.curState = eState.None;
        }
        else
        {
            state.curState = eState.Jump;
        }
    }

    public void CheckIsDie()
    {
        if (data.isDie == false && data.health <= 0 || transform.position.y < -2)
        {
            data.isDie = true;
            animator.SetBool("Die", data.isDie);
            Die();
        }
    }

    public void CheckIsReInk()
    {
        if (state.curFish == eInkFish.InkFish && state.curSame == eSame.Same)
        {
            data.isReInk = true;
        }
        else
            data.isReInk = false;
        
    }

    public void CheckIsInkLow()
    {
        if (data.ink <= 10)
        {
            data.isInkLow = true;
        }
        else
            data.isInkLow = false;
    }

    public void CheckInkFish()
    {
        if(state.curFish == eInkFish.InkFish && state.isMove == true)
        {
            var fish = model.inkFishModel.GetComponentInChildren<SkinnedMeshRenderer>(true);
            fish.gameObject.SetActive(true);
        }
        else
        {
            var fish = model.inkFishModel.GetComponentInChildren<SkinnedMeshRenderer>(true);
            fish.gameObject.SetActive(false);
        }
    }

    public void Move(float v,float h)
    {
        data.transform.position += data.transform.forward * v * data.moveSpeed * Time.deltaTime;

        data.transform.position += data.transform.right * h * data.moveSpeed * Time.deltaTime;
    }

    public void Rotate(float v)
    {
        var Angles = -v * data.turnSpeed * Time.deltaTime;
        data.transform.Rotate(0, Angles, 0);
    }

    public void RotateWeapon(float v)
    {
        var Angles = v * 100 * Time.deltaTime;
        Vector2 p = new Vector2(Screen.width / 2, Screen.height / 2);
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(p),out hit,Mathf.Infinity,layer))
        {
            model.muzzle.LookAt(hit.point);
        }

        CheckPosting(hit);
    }
    public void Jump()
    {
        if(state.curState == eState.Jump)
        {
            return;
        }
        Vector3 velocity = rigid.velocity;

        velocity.y = data.jumpHeight;

        rigid.velocity = velocity;
    }

    public void Shoot()
    {
        if (state.curFish == eInkFish.InkFish)
        {
            playerActor.Cmd2Human();
        }

        switch (state.curWeapon)
        {
            case eWeapon.Splattershot:
                if(data.shootTimer < data.splatterBlank || data.ink < 6)
                {
                    return;
                }
                if(isLocalPlayer)
                {
                    shootAudio.Play();

                    camera.ApplyRecoil(60, 0.2f);
                }
                data.shootTimer = 0;
                data.ink -= data.shootCost;

                GameObject shoot;
                shoot = Instantiate(model.mainVFX, model.muzzle.position, model.mainVFX.gameObject.transform.rotation, transform);
                shoot.gameObject.SetActive(true);
                Destroy(shoot.gameObject, 3);
                break;
            case eWeapon.Slosher:
                break;
            case eWeapon.Charger:
                break;
            case eWeapon.Roller:
                if (data.shootTimer < data.rollerBlank || data.ink < 6)
                {
                    return;
                }

                animator.SetTrigger("Slash");

                if(isLocalPlayer)
                    camera.ApplyRecoil(60, 0.2f);

                data.shootTimer = 0;
                data.ink -= data.rollerCost;
                Util.DelayCall(0.5f, () => 
                {
                    ParticleSystem roller;
                    roller = Instantiate(model.rollerVFX, model.muzzle.position, model.mainVFX.gameObject.transform.rotation, transform);
                    roller.gameObject.SetActive(true);
                    Destroy(roller.gameObject, 3);
                });

                break;
            default:
                break;
        }
    }

    public void TakeDamage(Actor atker,Actor target,Vector3 normal)
    {
        if (data.isDie)
            return;
        animator.SetTrigger("Hit");
        target.data.health -= atker.data.playerShellDamage;
        ParticleSystem atkVFX;
        atkVFX = Instantiate(model.underAtk, target.transform.position,Quaternion.Euler(-normal));
        atkVFX.gameObject.SetActive(true);
        Destroy(atkVFX.gameObject, 3);

        if(isLocalPlayer)
        {
            shake.PlayerUnderAttackShake();
            screenPost.gameObject.SetActive(true);
        }

        if (!isLocalPlayer)
            shake.AtkerShake();
    }

    float temp = 0f;
    float temp2 = 0f;
    public void TakeMapDamage(int damage)
    {
        temp = Time.time;

        if (temp - temp2 < 1)
            return;
        data.health -= damage;

        ParticleSystem atkVFX;
        atkVFX = Instantiate(model.underAtk, transform.position,Quaternion.Euler(0,1,0));
        atkVFX.gameObject.SetActive(true);
        Destroy(atkVFX.gameObject, 3);
        if (isLocalPlayer)
        {
            shake.PlayerUnderAttackShake();
            screenPost.gameObject.SetActive(true);
        }
        temp2 = temp;
    }

    [Command]
    public void CmdSetMapInfo(Vector2 rPos,int rColor)
    {
        RpcGetMapInfo(rPos, rColor);
    }
    [ClientRpc]
    public void RpcGetMapInfo(Vector2 rPos, int rColor)
    {
        if (!Mapping.map.ContainsKey(rPos))
        {
            Mapping.map.Add(rPos, (eColor)rColor);
        }
        else
        {
            Mapping.map[rPos] = (eColor)rColor;
        }
    }

    [Command]
    public void CmdRemoveMapInfo(Vector2 rPos)
    {
        RpcRemoveMapInfo(rPos);
    }
    [ClientRpc]
    public void RpcRemoveMapInfo(Vector2 rPos)
    {
        Mapping.map.Remove(rPos);
    }

    [Command]
    public void CmdAddV(Vector3 pos)
    {
        RpcAddV(pos);
    }
    [ClientRpc]
    public void RpcAddV(Vector3 pos)
    {
        Mapping.mapV.Add(pos);
    }

    public void Die()
    {
        ParticleSystem die;
        die = Instantiate(model.dieFX, transform);
        die.gameObject.SetActive(true);
        Destroy(die.gameObject, 1);
        if (isLocalPlayer)
            shake.DieShake();
    }


    public void Respawn(GameObject player)
    {
        data.health = data.healthMax;

        player.gameObject.SetActive(true);

        gameObject.transform.position = spawn;

        data.isDie = false;

        data.spawnTimer = 0;
    }

    public void RegenerateInk()
    {
        if(data.ink >= data.inkMax)
        {
            data.ink = data.inkMax;
            return;
        }

        data.ink += 0.1f;

        if (data.isReInk)
        {
            data.ink += 1f;
        }
    }

    public void RegenerateHealth()
    {
        if(data.health < data.healthMax)
        {
            data.health += 0.1f;
        }
    }

    public void AddPower()
    {
        if(data.power < data.powerMax)
            data.power += 1;
    }

    public void CheckRunVFX()
    {
        if (state.isMove == true && state.curSame == eSame.Same && state.curState != eState.Jump)
            model.runVFX.gameObject.SetActive(true);
        else
            model.runVFX.gameObject.SetActive(false);
    }

    public void AddFloorPost(Vector3 target)
    {
        ParticleSystem post;
        post = Instantiate(model.floorPost);
        post.transform.position = target;        
        post.gameObject.SetActive(true);
        Destroy(post.gameObject, 3);
    }

    public void AddPunchEffect(Vector3 target)
    {
        GameObject punch;
        punch = Instantiate(model.punch);
        punch.transform.position = target;
        punch.gameObject.SetActive(true);
    }

    public void DifEffect()
    {
        data.moveSpeed = data.difSpeed;
    }

    public void NormalEffect()
    {
        data.moveSpeed = data.normalSpeed;
    }

    public void CheckAnim()
    {
        if (isLocalPlayer)
        {
            animator.SetBool("Fire", state.isFire);
            animator.SetBool("Die", data.isDie);
            animator.SetFloat("MoveSpeed", controller.v);
            animator.SetFloat("RightSpeed", controller.h);

            if (state.curState == eState.Jump)
            {
                animator.SetBool("Jump", true);
            }
            else
                animator.SetBool("Jump", false);
        }
    }

    public void AddTransFX()
    {
        ParticleSystem trans;
        trans = Instantiate(model.transFX,transform);
        trans.gameObject.SetActive(true);
        Destroy(trans.gameObject, 2);
    }

    public void CheckPosting(RaycastHit hit)
    {
        if(hit.collider.tag == "Player")
        {
            state.isPosting = true;
            var uiAnim = camera.GetComponentInChildren<Animator>();
            uiAnim.SetBool("isPosting", state.isPosting);
        }
    }

    public void UseSubWeapon()
    {

    }

    public void UseSpecialWeapon()
    {
        if(data.power >= 100)
        {
            data.power -= 100;

            rigid.AddForce(new Vector3(0,5,0),ForceMode.Impulse);
            animator.SetTrigger("Power");
            Util.DelayCall(0.5f, () => 
            {
                GameObject spe;
                spe = Instantiate(model.powerVFX, transform);
                spe.SetActive(true);
                Destroy(spe.gameObject, 3);
            });
        }
    }
}
