using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorModel : MonoBehaviour
{
    public Transform muzzle;
    public Transform weapon;
    public GameObject humanModel;
    public GameObject clothing;
    public GameObject inkFishModel;

    public GameObject punch;

    public GameObject mainVFX;

    public ParticleSystem runVFX;
    public ParticleSystem underAtk;
    public ParticleSystem floorPost;
    public ParticleSystem transFX;
    public ParticleSystem dieFX;

    public Color One_Purple;
    public Color One_WarmYellow;
    public Color Two_LightBlue;
    public Color Two_ColdYellow;
    public Color Three_Green_Blue;
    public Color Three_Orange;
    public Color Four_Green_Yellow;
    public Color Four_Red_Purple;

    public Material m_One_Purple;
    public Material m_One_WarmYellow;
    public Material m_Two_LightBlue;
    public Material m_Two_ColdYellow;
    public Material m_Three_Green_Blue;
    public Material m_Three_Orange;
    public Material m_Four_Green_Yellow;
    public Material m_Four_Red_Purple;

    public Renderer inkBag;

    [HideInInspector] public static Color[] colors = { new Color(0.655f, 0.000f, 1.000f, 1.000f), new Color(1.000f, 0.860f, 0.404f, 1.000f), new Color(0.000f, 0.917f, 1.000f, 1.000f), new Color(1.000f, 0.981f, 0.301f, 1.000f), new Color(0.000f, 1.000f, 0.752f, 1.000f), new Color(1.000f, 0.613f, 0.199f, 1.000f), new Color(0.448f, 1.000f, 0.000f, 1.000f), new Color(1.000f, 0.303f, 0.606f, 1.000f) };
    //void Start ()
    //{

    //}

    //void Update()
    //{
    //    Debug.Log(One_Purple);
    //    Debug.Log(One_WarmYellow);
    //    Debug.Log(Two_LightBlue);
    //    Debug.Log(Two_ColdYellow);
    //    Debug.Log(Three_Green_Blue);
    //    Debug.Log(Three_Orange);
    //    Debug.Log(Four_Green_Yellow);
    //    Debug.Log(Four_Red_Purple);
    //}
}
