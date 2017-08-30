using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{
    Actor player;

    public GameObject inkLow;
    public GameObject reInk;
	void Start ()
    {
        player = GetComponent<Actor>();
	}
	
	void Update ()
    {
		
	}

    public void RegenerateInk()
    {

    }

}
