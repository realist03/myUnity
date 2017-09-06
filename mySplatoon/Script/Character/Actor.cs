using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEngine.UI;

public class Actor : NetworkBehaviour
{
    [HideInInspector]
    public ActorData data;
    [HideInInspector]
    public ActorModel model;

    public Vector3 spawn;

    public Image fillImage;

    Rigidbody rigid;

    CameraShake shake;

    PlayerCameraFreeLook camera;

    ChooseWeapon choose;

    public bool isMove = false;
    public bool isCharging = false;
    public float chargingTimer;

    bool isChoose = false;

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
        Fire,
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
    [SyncVar]
    public eInkFish curFish = eInkFish.Human;
    [SyncVar]
    public eColor curColor = eColor.None;
    [SyncVar]
    public eState curState = eState.None;
    [SyncVar]
    public eSame curSame = eSame.None;
    [SyncVar]
    public eWeapon curWeapon;

    public void Awake()
    {
        model = GetComponent<ActorModel>();
        rigid = GetComponent<Rigidbody>();
        data = GetComponent<ActorData>();
        shake = FindObjectOfType<CameraShake>();
        camera = FindObjectOfType<PlayerCameraFreeLook>();
        choose = FindObjectOfType<ChooseWeapon>();
    }



    public void Start()
    {
        spawn = gameObject.transform.position;
        gameObject.name = "Player" + netId.Value;
        Init();
        Debug.Log(netId.Value);

        Util.DelayCall(1f, () =>
        {
            CmdTeamId(netId.Value, data.TeamID);
        });
    }

    protected virtual void Init()
    {
        if (isLocalPlayer)
        {
            data.TeamID = BattleManager.Instance.curPlayerTeam;
        }
    }
    protected virtual void Update()
    {
        Debug.Log(netId.Value);
        if (GameMode.isGameOver == true)
            return;

        if (data.isDie)
        {
            return;
        }
        if (!GameMode.isReady)
            return;

        CheckMapColor();
        CheckCurJumpState();
        data.shootTimer += Time.deltaTime;

        //if (isLocalPlayer)
        //{
        //    CmdTeamId(netId.Value, data.TeamID);
        //}

        CheckIsInkLow();
        CheckIsReInk();
        CheckPainted();
        RegenerateInk();
        RegenerateHealth();
        CheckRunVFX();

        if (isLocalPlayer && GameMode.isReady)
        {
            curWeapon = (eWeapon)GameMode.tempWeaponID;

            CmdWeapon(netId.Value, curWeapon);
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
                curColor = eColor.One_Purple;
            }
            else
            {
                curColor = eColor.One_WarmYellow;
            }
        }
        else if(BattleManager.Instance.curColorPair == 2)
        {
            if (data.TeamID == 0)
            {
                curColor = eColor.Two_LightBlue;
            }
            else
            {
                curColor = eColor.Two_ColdYellow;
            }
        }
        else if (BattleManager.Instance.curColorPair == 3)
        {
            if (data.TeamID == 0)
            {
                curColor = eColor.Three_Green_Blue;
            }
            else
            {
                curColor = eColor.Three_Orange;
            }
        }
        else if (BattleManager.Instance.curColorPair == 4)
        {
            if (data.TeamID == 0)
            {
                curColor = eColor.Four_Green_Yellow;
            }
            else
            {
                curColor = eColor.Four_Red_Purple;
            }
        }
        Debug.Log(gameObject.name+"的颜色为"+curColor);

        SetCurColor();
    }

    public void SetCurColor()
    {
        switch (curColor)
        {
            case eColor.None:
                break;
            case eColor.One_Purple:
                var ps1 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps1.Length; i++)
                {
                    var  item = ps1[i].main;
                    item.startColor = model.One_Purple;

                    var post = ps1[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.One_Purple;
                    }
                    if(ps1[i].name == "ShellVFX3" || ps1[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps1[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_One_Purple;
                        }
                    }
                }

                fillImage.color = model.One_Purple;

                model.inkBag.sharedMaterial = model.m_One_Purple;
                break;
            case eColor.One_WarmYellow:
                var ps2 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps2.Length; i++)
                {
                    var item = ps2[i].main;
                    item.startColor = model.One_WarmYellow;

                    var post = ps2[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.One_WarmYellow;
                    }

                    if (ps2[i].name == "ShellVFX3" || ps2[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps2[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_One_WarmYellow;
                        }
                    }
                }

                fillImage.color = model.One_WarmYellow;

                model.inkBag.sharedMaterial = model.m_One_WarmYellow;
                break;
            case eColor.Two_LightBlue:
                var ps3 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps3.Length; i++)
                {
                    var item = ps3[i].main;
                    item.startColor = model.Two_LightBlue;

                    var post = ps3[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Two_LightBlue;
                    }

                    if (ps3[i].name == "ShellVFX3" || ps3[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps3[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Two_LightBlue;
                        }
                    }
                }

                fillImage.color = model.Two_LightBlue;

                model.inkBag.sharedMaterial = model.m_Two_LightBlue;
                break;
            case eColor.Two_ColdYellow:
                var ps4 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps4.Length; i++)
                {
                    var item = ps4[i].main;
                    item.startColor = model.Two_ColdYellow;

                    var post = ps4[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Two_ColdYellow;
                    }

                    if (ps4[i].name == "ShellVFX3" || ps4[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps4[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Two_ColdYellow;
                        }
                    }
                }

                fillImage.color = model.Two_ColdYellow;

                model.inkBag.sharedMaterial = model.m_Two_ColdYellow;
                break;
            case eColor.Three_Green_Blue:
                var ps5 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps5.Length; i++)
                {
                    var item = ps5[i].main;
                    item.startColor = model.Three_Green_Blue;

                    var post = ps5[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Three_Green_Blue;
                    }

                    if (ps5[i].name == "ShellVFX3" || ps5[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps5[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Three_Green_Blue;
                        }
                    }
                }

                fillImage.color = model.Three_Green_Blue;

                model.inkBag.sharedMaterial = model.m_Three_Green_Blue;
                break;
            case eColor.Three_Orange:
                var ps6 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps6.Length; i++)
                {
                    var item = ps6[i].main;
                    item.startColor = model.Three_Orange;

                    var post = ps6[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Three_Orange;
                    }

                    if (ps6[i].name == "ShellVFX3" || ps6[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps6[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Three_Orange;
                        }
                    }
                }

                fillImage.color = model.Three_Orange;

                model.inkBag.sharedMaterial = model.m_Three_Orange;
                break;
            case eColor.Four_Green_Yellow:
                var ps7 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps7.Length; i++)
                {
                    var item = ps7[i].main;
                    item.startColor = model.Four_Green_Yellow;

                    var post = ps7[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Four_Green_Yellow;
                    }

                    if (ps7[i].name == "ShellVFX3" || ps7[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps7[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Four_Green_Yellow;
                        }
                    }
                }

                fillImage.color = model.Four_Green_Yellow;

                model.inkBag.sharedMaterial = model.m_Four_Green_Yellow;
                break;
            case eColor.Four_Red_Purple:
                var ps8 = model.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps8.Length; i++)
                {
                    var item = ps8[i].main;
                    item.startColor = model.Four_Red_Purple;

                    var post = ps8[i].GetComponent<DecalsPost>();
                    if (post != null)
                    {
                        var tex = post.prints[0].gameObject.GetComponent<Decal>();
                        tex.AlbedoColor = model.Four_Red_Purple;
                    }

                    if (ps8[i].name == "ShellVFX3" || ps8[i].name == "ShellVFX3_2")
                    {
                        var post_m = ps8[i].gameObject.GetComponent<Renderer>();
                        if (post_m != null)
                        {
                            post_m.sharedMaterial = model.m_Four_Red_Purple;
                        }
                    }
                }

                fillImage.color = model.Four_Red_Purple;

                model.inkBag.sharedMaterial = model.m_Four_Red_Purple;
                break;
            default:
                break;
        }
    }

    public void InitWeapon(eWeapon curWeapon)
    {
        if (GameMode.isReady == false)
            return;
        if (isChoose == true)
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
                Instantiate(Resources.Load<GameObject>("Prefab/Weapon/Weapon-Roller"), new Vector3(model.weapon.position.x, model.weapon.position.y-0.2f, model.weapon.position.z+1f), Quaternion.Euler(-30, 180, 90), model.weapon);
                break;
            default:
                break;
        }
        isChoose = true;
    }
    public void CheckMapColor()
    {
        var posX = Mathf.FloorToInt(transform.position.x);
        var posY = Mathf.FloorToInt(transform.position.z);

        eColor mapColor;

        if (Mapping.map.ContainsKey(new Vector2(posX, posY)))
        {
            //Debug.Log(Mapping.map.Count);
            if (Mapping.map.TryGetValue(new Vector2(posX, posY), out mapColor))
            {
                if (mapColor != eColor.None && mapColor != curColor)
                {
                    curSame = eSame.Diffent;
                    TakeMapDamage(10);

                    DifEffect();
                }
                else
                    NormalEffect();

                if (mapColor != eColor.None && mapColor == curColor)
                {
                    curSame = eSame.Same;
                    //Debug.Log("same");
                }
            }
        }
        else
            curSame = eSame.None;
    }

    public void CheckPainted()
    {
        if (curFish == eInkFish.InkFish && curSame == eSame.Same)
        {
            data.moveSpeed = data.rushSpeed;
            data.jumpHeight = data.rushJumpHeight;
        }
        else
        {
            data.moveSpeed = data.normalSpeed;
            data.jumpHeight = data.normalHeight; ;
        }
    }

    public void CheckCurJumpState()
    {
        if (Physics.Raycast(transform.position, -transform.up, 1))
        {
            if(curState == eState.Jump)
            {
                AddFloorPost(transform.position);
            }
            curState = eState.None;
        }
        else
        {
            curState = eState.Jump;
        }
    }

    public void CheckIsDie()
    {
        if (!data.isDie && data.health <= 0)
        {
            data.isDie = true;

            Die();
        }
    }

    public void CheckIsReInk()
    {
        if (curFish == eInkFish.InkFish && curSame == eSame.Same)
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

    public void Move(float v,float h)
    {
        data.transform.position += data.transform.forward * v * data.moveSpeed * Time.deltaTime;

        data.transform.position += data.transform.right * h * data.moveSpeed * Time.deltaTime;
    }

    [Command]
    public void CmdMove(float v, float h)
    {
        RpcMove(v, h);
    }
    [ClientRpc]
    public void RpcMove(float v, float h)
    {
        data.transform.position += data.transform.forward * v * data.moveSpeed * Time.deltaTime;

        data.transform.position += data.transform.right * h * data.moveSpeed * Time.deltaTime;
    }

   

    public void Rotate(float v)
    {
        var Angles = -v * data.turnSpeed * Time.deltaTime;
        data.transform.Rotate(0, Angles, 0);
    }
    [Command]
    public void CmdRotate(float v)
    {
        RpcRotate(v);
    }
    [ClientRpc]
    public void RpcRotate(float v)
    {
        var Angles = -v * data.turnSpeed * Time.deltaTime;
        data.transform.Rotate(0, Angles, 0);
    }

    public void Jump()
    {
        if(curState == eState.Jump)
        {
            return;
        }
        Vector3 velocity = rigid.velocity;

        velocity.y = data.jumpHeight;

        rigid.velocity = velocity;
    }
    [Command]
    public void CmdJump()
    {
        RpcJump();
    }
    [ClientRpc]
    public void RpcJump()
    {
        if (curState == eState.Jump)
        {
            return;
        }
        Vector3 velocity = rigid.velocity;

        velocity.y = data.jumpHeight;

        rigid.velocity = velocity;
    }

    public void Shoot()
    {
        if(curState == eState.Fire || curFish == eInkFish.InkFish || data.shootTimer < data.shootBlank || data.ink < 6)
        {
            return;
        }

        switch (curWeapon)
        {
            case eWeapon.Splattershot:
                camera.ApplyRecoil(60, 0.2f);
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
                if(chargingTimer >= 2)
                {

                }
                break;
            default:
                break;
        }
    }
    [Command]
    public void CmdShoot()
    {
        RpcShoot();
    }
    [ClientRpc]
    public void RpcShoot()
    {
        if (curState == eState.Fire || curFish == eInkFish.InkFish || data.shootTimer < data.shootBlank || data.ink < 6)
        {
            return;
        }

        switch (curWeapon)
        {
            case eWeapon.Splattershot:
                camera.ApplyRecoil(60, 0.2f);
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
                break;
            default:
                break;
        }
    }

    public void TakeDamage(Actor atker,Actor target,Vector3 normal)
    {

        target.data.health -= atker.data.playerShellDamage;
        ParticleSystem atkVFX;
        atkVFX = Instantiate(model.underAtk, target.transform.position,Quaternion.Euler(-normal));
        atkVFX.gameObject.SetActive(true);
        Destroy(atkVFX.gameObject, 3);
        CheckIsDie();
        if(isLocalPlayer)
            shake.PlayerUnderAttackShake();
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
        CheckIsDie();
        if (isLocalPlayer)
            shake.PlayerUnderAttackShake();
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

    private Renderer[] renders;

    public void Die()
    {

        renders = gameObject.GetComponentsInChildren<Renderer>();

        foreach (var render in renders)
        {
            render.gameObject.SetActive(false);
        }

        Util.DelayCall(7, () => 
        {
            Respawn();
        });
    }


    public void Respawn()
    {
        data.health = data.healthMax;
        data.isDie = false;
        gameObject.transform.position = spawn;

        foreach (var render in renders)
        {
            render.gameObject.SetActive(true);
        }
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
            data.power += 10;
    }

    public void CheckRunVFX()
    {
        if (isMove == true && curSame == eSame.Same && curState != eState.Jump)
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
}
