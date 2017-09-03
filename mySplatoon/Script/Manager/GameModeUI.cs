using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeUI : MonoBehaviour
{
    public Text gameTime;
    public Text winTeam;

    public Text team1Points;
    public Text team2Points;
	void Start ()
    {
		
	}
	
	void Update ()
    {
        gameTime.text = "Time:" + ((int)GameMode.gameTime).ToString();

        if(GameMode.isGameOver == true)
        {
            if (GameMode.isTeam1Win)
            {
                winTeam.gameObject.SetActive(true);
                winTeam.text = "TeamA Win!";
                team1Points.gameObject.SetActive(true);
                team2Points.gameObject.SetActive(true);
                team1Points.text = GameMode.Team1Points.ToString();
                team2Points.text = GameMode.Team2Points.ToString();
            }
            else
            {
                winTeam.gameObject.SetActive(true);
                winTeam.text = "TeamB Win!";
                team1Points.gameObject.SetActive(true);
                team2Points.gameObject.SetActive(true);
                team1Points.text = GameMode.Team1Points.ToString();
                team2Points.text = GameMode.Team2Points.ToString();
            }
        }
    }
}
