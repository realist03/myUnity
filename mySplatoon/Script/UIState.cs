using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIState : MonoBehaviour
{
    public Color Red;
    public Color Blue;

    public GameObject InkLow;
    public GameObject ReInk;

    public Slider inkValue;
    public Image InkFill;

    PlayerCharacter player;

	void Start ()
    {
        player = FindObjectOfType<PlayerCharacter>();
	}
	
	void Update ()
    {
        inkValue.value = player.ink;
<<<<<<< HEAD

=======
>>>>>>> 4966e6608649f9cd36703951aa5c474a412625c7
        if (player.ink <= 10)
        {
            InkLow.SetActive(true);
        }
        else
        {
            InkLow.SetActive(false);
        }

        if (player.isReInk)
        {
            ReInk.SetActive(true);
        }
        else
        {
            ReInk.SetActive(false);
        }

        if (player.curColor == Character.chaColor.Red)
        {
            InkFill.color = Red;
        }
        else
        {
            InkFill.color = Blue;
        }
	}
}
