using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour {
    public GameScenesManager gameScenesManager;
    public Button startGame;
    public Button quitGame;
    public DOTweenAnimation GameStart;
    public DOTweenAnimation start;
    public DOTweenAnimation exit;
    
    
    private void Start()
    {
        
      
        startGame.onClick.AddListener(AC_ChoiceScene);
        quitGame.onClick.AddListener(AC_QuitGame);

    }

    private void AC_QuitGame()
    {
       
       
        
        Debug.Log("ExitGame");
        Application.Quit();
    }

    private void AC_ChoiceScene()
    {
        GameStart.DOPlay();
        gameScenesManager.LoadingScenes(GameScenesManager.SCENESTATE.ChoiceScene);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name!="Start")
        {
            gameObject.SetActive(false);
        }
        
        
    }






}
