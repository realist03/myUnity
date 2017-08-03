using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;

    //持续抖动的时长
    public float shake = 0f;

    // 抖动幅度（振幅）
    //振幅越大抖动越厉害
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    //void OnEnable()
    //{
    //    originalPos = camTransform.localPosition;
    //}

    void Update()
    {
        if (shake > 0)
        {
            camTransform.localPosition = camTransform.position + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;
        }
        if(shake < 0.01)
        {
            shake = 0;
        }
        //else
        //{
        //    shake = 0f;
        //    camTransform.localPosition = originalPos;
        //}
    }

    public void PlayerUnderAttackShake()
    {
        shake = 0.7f;
        shakeAmount = 0.5f;
        decreaseFactor = 1.0f;
    }

    public void EnemytDieShake()
    {
        shake = 0.5f;
        shakeAmount = 0.1f;
        decreaseFactor = 1.5f;
    }
}
