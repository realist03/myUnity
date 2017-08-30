using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BattleManager 
{
    private static BattleManager _Instance;
    public static BattleManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new BattleManager();
            }

            return _Instance;
        }
    }



    public int curPlayerTeam;

    public int curGameModel;

    public int curColorPair;


    public Dictionary<uint, Actor> curBattlePlayerDict = new Dictionary<uint, Actor>();

}
