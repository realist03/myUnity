using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameModeUI : MonoBehaviour
{
    public Text gameTime;
    public Text winTeam;

    public Slider team1Points;
    public Slider team2Points;

    public Image team1Fill;
    public Image team2Fill;

    public GameObject gameTimeUp;

    public Animator animator1;
    public Animator animator2;

    public GameObject judds;

    private void Start()
    {
        InitUIColor();
    }
    void Update ()
    {
        gameTime.text = ((int)GameMode.gameTime).ToString();

        if(GameMode.isGameOver == true)
        {
            gameTimeUp.SetActive(true);

            Util.DelayCall(5, () =>
            {
                judds.SetActive(true);

                team1Points.gameObject.SetActive(true);
                team2Points.gameObject.SetActive(true);
                team1Points.maxValue = 1;
                team2Points.maxValue = 1;

                Debug.Log("tP:" + GameMode.totalPoints);

                Debug.Log("t1P:" + GameMode.Team1Points);
                Debug.Log("t2P:" + GameMode.Team2Points);

                Debug.Log("t1max:" + team1Points.maxValue);
                Debug.Log("t2max:" + team2Points.maxValue);

                Util.DelayCall(3, () =>
                {

                    DOTween.To(() => team1Points.value, x => team1Points.value = x, team1Points.value, 5).OnUpdate(() =>
                    {
                        if (team1Points.value < (GameMode.Team1Points / GameMode.totalPoints))
                            team1Points.value += Time.deltaTime;
                    });
                        Debug.Log("t1:" + team1Points.value);

                    DOTween.To(() => team2Points.value, x => team2Points.value = x, team2Points.value, 5).OnUpdate(() =>
                    {
                        if (team2Points.value < (GameMode.Team2Points / GameMode.totalPoints))
                            team2Points.value += Time.deltaTime;
                    });
                        Debug.Log("t2:" + team2Points.value);
                });
            });

            if (GameMode.isTeam1Win == true)
            {
                Util.DelayCall(8, () =>
                {
                    animator1.SetTrigger("AWin");
                    animator2.SetTrigger("BLose");

                    winTeam.text = "TeamA Win!";
                    winTeam.gameObject.SetActive(true);
                });
            }
            if (GameMode.isTeam1Win == false)
            {
                Util.DelayCall(8, () =>
                {
                    animator1.SetTrigger("ALose");
                    animator2.SetTrigger("BWin");

                    winTeam.text = "TeamB Win!";
                    winTeam.gameObject.SetActive(true);
                });
            }
        }
    }

    void InitUIColor()
    {
        if(BattleManager.Instance.curColorPair == 1)
        {
            team1Fill.color = ActorModel.colors[0];
            team2Fill.color = ActorModel.colors[1];
        }
        if (BattleManager.Instance.curColorPair == 2)
        {
            team1Fill.color = ActorModel.colors[2];
            team2Fill.color = ActorModel.colors[3];
        }
        if (BattleManager.Instance.curColorPair == 3)
        {
            team1Fill.color = ActorModel.colors[4];
            team2Fill.color = ActorModel.colors[5];
        }
        if (BattleManager.Instance.curColorPair == 4)
        {
            team1Fill.color = ActorModel.colors[6];
            team2Fill.color = ActorModel.colors[7];
        }

    }
}
