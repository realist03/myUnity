using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotate : MonoBehaviour
{
    float turnSpeed = 200;
    
    private void Update()
    {
        var y = Input.GetAxis("Mouse Y");

        var yAngles = y * turnSpeed * Time.deltaTime;

        transform.Rotate(-yAngles, 0, 0);
    }
}
