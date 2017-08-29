using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class GameMode : NetworkBehaviour
{
    public static bool isGameOver = false;

    public float GameTime;

    public int Team1Points;
    public int Team2Points;

    public LobbyPlayer lobby;

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
    }

    void Start()
    {
        curMode = (eMode)lobby.gameMode;
    }

    void Update()
    {
        Debug.Log(curMode);
        switch (curMode)
        {
            case eMode.Floor:
                GameTime -= Time.deltaTime;
                if (GameTime == 0)
                {
                    foreach (var item in Mapping.map)
                    {

                    }
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
