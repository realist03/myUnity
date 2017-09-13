using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using DG.Tweening;

public class GameMode : NetworkBehaviour
{
    public LobbyPlayer lobby;
    public AudioSource whistle;
    public AudioClip whistling;
    public AudioSource BGM;
    public GameObject playerCamera;
    public GameObject scenceCamera;
    public GameObject gameModeUI;
    public GameObject black;

    public static bool isGameOver = false;
    public static float gameTime = 180;
    public float setGameTime;
    public static int Team1Points = 0;
    public static int Team2Points = 0;
    public static int totalPoints = 0;
    public static bool isTeam1Win;

    public static bool isReady = false;


    public static int tempWeaponID;
    GameObject[] actors;

    bool isAdd = false;

    public enum eMode
    {
        Floor,
        Car,
        Fish,
        Occupy,
    }
    
    public eMode curMode;

    private void Awake()
    {
        lobby = FindObjectOfType<LobbyPlayer>();

        gameTime = setGameTime;
        whistling = GetComponent<AudioClip>();
    }

    void Start()
    {
        actors = GameObject.FindGameObjectsWithTag("Player");

        curMode = (eMode)lobby.gameMode;
        whistle.Play();

        BGM.Play();
    }

    void Update()
    {
        Debug.Log(curMode);
        Debug.Log(isGameOver);

        if(!isGameOver)
        {
            switch (curMode)
            {
                case eMode.Floor:
                    if(isGameOver == false)
                    {
                        gameTime -= Time.deltaTime;
                    }

                    if (gameTime <= 0)
                    {
                        whistle.Play();

                        BGM.volume = 0.05f;

                        isGameOver = true;
                    }
                    break;
                case eMode.Car:
                    break;
                case eMode.Fish:
                    break;
                case eMode.Occupy:
                    break;
                default:
                    break;
            }
        }
        else
        {
            AddCount();
            Util.DelayCall(2, () =>
            {
                black.SetActive(true);
            });


            Util.DelayCall(3, () =>
             {
                 gameModeUI.SetActive(false);
                 playerCamera.SetActive(false);
                 scenceCamera.SetActive(true);
                 for (int i = 0; i < actors.Length; i++)
                 {
                     actors[i].SetActive(false);
                 }
             });

        }
        
    }

    void AddCount()
    {
        if (isAdd == false)
        {
            foreach (var item in Mapping.map)
            {
                totalPoints += 1;
                if (item.Value == Actor.eColor.One_Purple || item.Value == Actor.eColor.Two_LightBlue || item.Value == Actor.eColor.Three_Green_Blue || item.Value == Actor.eColor.Four_Green_Yellow)
                {
                    Team1Points += 1;
                }
                else
                {
                    Team2Points += 1;
                }
            }
            isAdd = true;
        }

        if (Team1Points > Team2Points)
        {
            isTeam1Win = true;
        }
        else
            isTeam1Win = false;

    }

    void Init()
    {
        isGameOver = false;
        gameTime = setGameTime;
        Team1Points = 0;
        Team2Points = 0;
        isReady = false;
    }

}
