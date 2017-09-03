using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UILogic : NetworkBehaviour
{
    Actor player;

    public GameObject inkLow;
    public GameObject inkObj;
    public Slider reInk;

    public Transform Bag;

	void Start ()
    {
        player = GetComponent<Actor>();
	}
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        Bag.localScale = new Vector3(1.35788f, 1.374861f * (player.data.ink / player.data.inkMax), 1.357888f);

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
