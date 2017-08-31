using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogic : MonoBehaviour
{
    Actor player;

    public GameObject inkLow;
    public GameObject inkObj;
    public Slider reInk;

	void Start ()
    {
        player = GetComponent<Actor>();
	}
	
	void Update ()
    {
        if (player.data.isReInk)
        {
            inkObj.SetActive(true);
            reInk.value = player.data.ink;
        }
        else
        {
            inkObj.SetActive(false);
        }

        if (player.data.isInkLow)
        {
            inkLow.SetActive(true);
        }
        else
            inkLow.SetActive(false);
	}
}
