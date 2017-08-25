using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public static bool isGameOver = false;

    public static int ColorPair;

	void Start ()
    {
        ColorPair = Random.Range(1, 4);
	}
	
	void Update ()
    {
		
	}
}
