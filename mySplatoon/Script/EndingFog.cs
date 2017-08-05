using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingFog : MonoBehaviour
{
    EndingFog ending;

    public Rigidbody player;
    public Camera camera;
    public float timer = 120;
    public float speed = 8;
	void Start ()
    {
	}
	
	void FixedUpdate ()
    {
        camera.fieldOfView += Time.deltaTime * speed;
        timer -= Time.deltaTime;
        player.transform.position += new Vector3(0, 0, 0.05f);

        if(camera.fieldOfView >= 160)
        {
            camera.fieldOfView = 160;
        }
	}
}
