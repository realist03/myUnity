using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DecalsFade : MonoBehaviour
{
    
	void Start ()
    {
        var range = UnityEngine.Random.Range(2.7f, 3.1f);

        var decal = GetComponent<Decal>();
        decal.transform.localScale = Vector3.one * 1f;

        decal.transform.DOScale(Vector3.one * range, 0.2f);
    }
}
