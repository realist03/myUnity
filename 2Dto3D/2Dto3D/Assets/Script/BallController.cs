using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Ball ball;
	void Start ()
    {
		ball = GetComponent<Ball>();
	}
	
	void Update ()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
            ball.Move(h);
            ball.Jump(v);
    }
}
