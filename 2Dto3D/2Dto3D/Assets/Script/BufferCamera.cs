using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferCamera : MonoBehaviour
{

    public Transform target;
    public float height;
    public float distance;
    public float Interpolation = 1f;

    void Start()
    {
    }

    void LateUpdate()
    {
        var lerpAngles = Mathf.LerpAngle(transform.eulerAngles.y, target.eulerAngles.y, Interpolation * Time.deltaTime);
        var currentRotation = Quaternion.Euler(0, lerpAngles, 0);
        var currentdistance = currentRotation * Vector3.forward * distance;
        var pos = target.position - currentdistance;

        transform.position = new Vector3(pos.x, target.position.y + height, pos.z);
        transform.LookAt(target);
        if(Ball.is3D == true)
        {
            transform.LookAt(null);
        }
        else if(Ball.is3D == false)
        {
            transform.LookAt(target);
        }
    }
}
