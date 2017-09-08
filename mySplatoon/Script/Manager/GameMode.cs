using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class GameMode : NetworkBehaviour
{
    public static bool isGameOver = false;
    public static float gameTime = 180;
    public float setGameTime;
    public static int Team1Points = 0;
    public static int Team2Points = 0;

    public static bool isTeam1Win;

    public static bool isReady = false;

    public LobbyPlayer lobby;

    public static int tempWeaponID;
    Actor actor;

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

        actor = FindObjectOfType<Actor>();
        gameTime = setGameTime;
    }

    void Start()
    {
        curMode = (eMode)lobby.gameMode;
    }

    void Update()
    {
        Debug.Log(curMode);
        Debug.Log(isGameOver);
        switch (curMode)
        {
            case eMode.Floor:
                if(isGameOver == false)
                {
                    gameTime -= Time.deltaTime;
                }

                if (gameTime <= 0)
                {
                    if(isAdd == false)
                    {
                        foreach (var item in Mapping.map)
                        {
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
}
