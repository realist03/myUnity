using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Actor : NetworkBehaviour
{
    [HideInInspector]
    public ActorData data;
    [HideInInspector]
    public ActorModel model;

    Rigidbody rigid;

    public enum eColor
    {
        None,

        One_Purple,
        One_WarmYellow,

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
        Move,
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
    public eInkFish curFish = eInkFish.Human;
    public eColor curColor = eColor.None;
    public eState curState = eState.None;
    public eSame curSame = eSame.None;

    private void Start()
    {
        Init();
    }

    protected virtual void Init ()
    {
        model = GetComponent<ActorModel>();
        rigid = GetComponent<Rigidbody>();
        data = GetComponent<ActorData>();
        InitCurColor();
	}

    protected virtual void Update ()
    {
        CheckMapColor();
        CheckCurJumpState();
        SetCurColor();
        data.shootTimer += Time.deltaTime;
    }

    public void InitCurColor()
    {
        if(GameMode.ColorPair == 1)
        {
            if(data.TeamID == 1)
            {
                curColor = eColor.One_Purple;
            }
            else
            {
                curColor = eColor.One_WarmYellow;
            }
        }
        else if(GameMode.ColorPair == 2)
        {
            if (data.TeamID == 1)
            {
                curColor = eColor.Two_LightBlue;
            }
            else
            {
                curColor = eColor.Two_ColdYellow;
            }
        }
        else if (GameMode.ColorPair == 3)
        {
            if (data.TeamID == 1)
            {
                curColor = eColor.Three_Green_Blue;
            }
            else
            {
                curColor = eColor.Three_Orange;
            }
        }
        else if (GameMode.ColorPair == 4)
        {
            if (data.TeamID == 1)
            {
                curColor = eColor.Four_Green_Yellow;
            }
            else
            {
                curColor = eColor.Four_Red_Purple;
            }
        }
    }

    public void SetCurColor()
    {
        switch (curColor)
        {
            case eColor.None:
                break;
            case eColor.One_Purple:
                var op = model.MainVFX.main;
                op.startColor = model.One_Purple;

                var post1 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex1 = post1.prints[0].gameObject.GetComponent<Decal>();
                tex1.AlbedoColor = model.One_Purple;
                break;
            case eColor.One_WarmYellow:
                var ow = model.MainVFX.main;
                ow.startColor = model.One_WarmYellow;

                var post2 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex2 = post2.prints[0].gameObject.GetComponent<Decal>();
                tex2.AlbedoColor = model.One_WarmYellow;
                break;
            case eColor.Two_LightBlue:
                var tl = model.MainVFX.main;
                tl.startColor = model.Two_LightBlue;

                var post3 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex3 = post3.prints[0].gameObject.GetComponent<Decal>();
                tex3.AlbedoColor = model.Two_LightBlue;
                break;
            case eColor.Two_ColdYellow:
                var tc = model.MainVFX.main;
                tc.startColor = model.Two_ColdYellow;

                var post4 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex4 = post4.prints[0].gameObject.GetComponent<Decal>();
                tex4.AlbedoColor = model.Two_ColdYellow;
                break;
            case eColor.Three_Green_Blue:
                var tgb = model.MainVFX.main;
                tgb.startColor = model.Three_Green_Blue;

                var post5 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex5 = post5.prints[0].gameObject.GetComponent<Decal>();
                tex5.AlbedoColor = model.Three_Green_Blue;
                break;
            case eColor.Three_Orange:
                var to = model.MainVFX.main;
                to.startColor = model.Three_Orange;

                var post6 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex6 = post6.prints[0].gameObject.GetComponent<Decal>();
                tex6.AlbedoColor = model.Three_Orange;
                break;
            case eColor.Four_Green_Yellow:
                var fgy = model.MainVFX.main;
                fgy.startColor = model.Four_Green_Yellow;

                var post7 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex7 = post7.prints[0].gameObject.GetComponent<Decal>();
                tex7.AlbedoColor = model.Four_Green_Yellow;
                break;
            case eColor.Four_Red_Purple:
                var frp = model.MainVFX.main;
                frp.startColor = model.Four_Red_Purple;

                var post8 = model.MainVFX.gameObject.GetComponent<DecalsPost>();
                var tex8 = post8.prints[0].gameObject.GetComponent<Decal>();
                tex8.AlbedoColor = model.Four_Red_Purple;
                break;
            default:
                break;
        }
    }

    public void CheckMapColor()
    {
        var posX = Mathf.FloorToInt(transform.position.x);
        var posY = Mathf.FloorToInt(transform.position.z);

        eColor mapColor;

        if (Mapping.map.ContainsKey(new Vector2(posX, posY)))
        {
            if (Mapping.map.TryGetValue(new Vector2(posX, posY), out mapColor))
            {
                if (mapColor != eColor.None && mapColor != curColor)
                {
                    curSame = eSame.Diffent;
                }
                if (mapColor != eColor.None && mapColor == curColor)
                {
                    curSame = eSame.Same;
                }
            }
        }
        else
            curSame = eSame.None;
    }

    public void CheckPainted()
    {
        if(curFish == eInkFish.InkFish && curSame == eSame.Same)
        {
            data.moveSpeed = data.rushSpeed;
            data.jumpHeight = data.rushJumpHeight;
        }
    }

    public void CheckCurJumpState()
    {
        if (Physics.Raycast(transform.position, -transform.up, 1))
        {
            curState = eState.None;
        }
        else
        {
            curState = eState.Jump;
        }
    }



    public void Move(float v,float h)
    {
        data.transform.position += data.transform.forward * v * data.moveSpeed * Time.deltaTime;

        data.transform.position += data.transform.right * h * data.moveSpeed * Time.deltaTime;
    }

    [Command]
    public void CmdMove(float v, float h)
    {
        data.transform.position += data.transform.forward * v * data.moveSpeed * Time.deltaTime;

        data.transform.position += data.transform.right * h * data.moveSpeed * Time.deltaTime;
    }

    public void Rotate(float v)
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

    public void Shoot()
    {
        if(curState == eState.Fire || curFish == eInkFish.InkFish || data.shootTimer < data.shootBlank)
        {
            return;
        }

        data.shootTimer = 0;

        ParticleSystem shoot;

        shoot = Instantiate(model.MainVFX, model.muzzle.position, model.MainVFX.gameObject.transform.rotation, transform) as ParticleSystem;
        shoot.gameObject.SetActive(true);
        Destroy(shoot, 1);
    }
}
