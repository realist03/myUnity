using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameState : MonoBehaviour
{
    public enum GAMESTATE
    {
        start = 1,
        ChoiceScene,
        loading,
        game,
    }
    public static GAMESTATE gamestate;

    public static int score;

    private static GameState instance;

    public static GameState Instance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType<GameState>();
        }
        if (!instance)
        {
            Debug.LogError("There needs to be one active GameState Script on a GameObject in your scene");
        }
        return instance;
    }


    public static void LoadScene(GAMESTATE _gamestate) { 
        switch (_gamestate)
        {
            case GAMESTATE.start:
                SceneManager.LoadScene("Start");
                break;
            case GAMESTATE.ChoiceScene:
                SceneManager.LoadScene("Chose",LoadSceneMode.Single);
                break;
            case GAMESTATE.loading:
                break;
            case GAMESTATE.game:
                SceneManager.LoadScene("Game");
                break;

            default:
                break;
        }
    }

    private void Awake()
    {
        Object.DontDestroyOnLoad(transform.gameObject);
        gamestate = GAMESTATE.start;
    }
}
