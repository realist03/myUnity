using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DecalsFade : MonoBehaviour
{

	void Start ()
    {
        transform.localScale = Vector3.one* 0.5f;

        transform.DOScale(Vector3.one,0.3f);
    }
	
	 
}
