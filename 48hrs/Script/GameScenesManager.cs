using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameScenesManager : MonoBehaviour
{
   public  DOTweenAnimation londing;
    public Text lodingText;
    public enum SCENESTATE
    {
        start = 1,
        ChoiceScene,
        game,
        Tutorials,
        end,
    }
    public Image startImage;   

    private static GameScenesManager instance;

    public static GameScenesManager Instance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType<GameScenesManager>();
        }
        if (!instance)
        {
            Debug.LogError("There needs to be one active GameScenesManager Script on a GameObject in your scene");
        }
        return instance;
    }


    private void Awake()
    {
        Object.DontDestroyOnLoad(transform.gameObject);
        

    }

    /// <summary>
    /// 传入一个SCENESTATE，加载对应的场景
    /// </summary>
    /// <param name="gamestate"></param>
    public void LoadingScenes(SCENESTATE gamestate)
    {
        switch (gamestate)
        {
            case SCENESTATE.start:
                StartCoroutine(StartLoading("Start", lodingText));
                break;
            case SCENESTATE.ChoiceScene:
                StartCoroutine(StartLoading("Chose", lodingText));
                break;
            case SCENESTATE.Tutorials:
                StartCoroutine(StartLoading("Tutorials", lodingText));
                break;
            case SCENESTATE.game:
                StartCoroutine(StartLoading("Game", lodingText));
                break;
            case SCENESTATE.end:
                StartCoroutine(StartLoading("Ending", lodingText));
                break;
            default:
                break;
        }
    }
    public  IEnumerator StartLoading(string ScenesName, Text lodingText)
    {
        lodingText.gameObject.SetActive(true);
        londing.DOPlay();
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(ScenesName);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress;

            while (displayProgress < toProgress)
            {
                ++displayProgress;

                lodingText.text = displayProgress.ToString() + "%";
                yield return new WaitForEndOfFrame();

            }

        }
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;

            lodingText.text = displayProgress.ToString() + "%";
            yield return new WaitForEndOfFrame();
            
        }

        op.allowSceneActivation = true;
        lodingText.gameObject.SetActive(false);
    }


}


