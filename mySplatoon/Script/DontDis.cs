using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDis : MonoBehaviour {

    private void Awake()
    {
        //HUD一直存在，需要隐藏的时候隐藏掉
        DontDestroyOnLoad(transform.gameObject);
    }
}
