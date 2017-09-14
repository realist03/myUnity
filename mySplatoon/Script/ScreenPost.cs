using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPost : MonoBehaviour
{
    float timer;
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer>=10)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
	}
}
