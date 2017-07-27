using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static bool is3D = true;

    public float runSpeed;
    public float moveSpeed;
    public float jumpForce;
    Rigidbody rigid;
    public GameObject mainCamera;
    public GameObject secondCamera;
    public GameObject icon;
    public GameObject FX;
    public Transform enter2D;
    public Transform enter3D;
    public Transform to2D;
	void Start ()
    {
        secondCamera.SetActive(false);
        rigid = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
		rigid.AddForce(new Vector3(runSpeed, 0,0));
        if(is3D == true)
        {
            //第二相机跟随第一相机
            secondCamera.transform.position = mainCamera.transform.position;
            secondCamera.transform.rotation = mainCamera.transform.rotation;
        }
    }

    public void TransDimensions() //转换维度
    {
        if(is3D == true)
        {
            is3D = false;
            //球和特效的layer
            icon.gameObject.layer = 8;
            FX.gameObject.layer = 8;
            //开关相机
            mainCamera.SetActive(false);
            secondCamera.SetActive(true);
            //第二相机目标旋转
            to2D.Rotate(new Vector3(0, -90, 0));
            //传送
            transform.position = enter2D.position;
        }
        else if(is3D == false)
        {
            is3D = true;

            //球和特效的layer
            icon.gameObject.layer = 4;
            FX.gameObject.layer = 4;

            //开关相机
            mainCamera.SetActive(true);
            secondCamera.SetActive(false);

            //第二相机目标旋转回初始位置
            to2D.Rotate(new Vector3(0, 90, 0));

            //传送
            transform.position = enter3D.position;
        }

    }

    public void Move(float mS)
    {
        if(!is3D)
        {
            rigid.AddForce(new Vector3(mS * moveSpeed, 0, 0));
        }
        else
        {
            rigid.AddForce(new Vector3(0, 0, -mS * moveSpeed));
        }
    }

    public void Jump(float tS)
    {
        rigid.AddForce(new Vector3(0, tS * jumpForce, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "transD")
        {
            TransDimensions();
        }
    }
}
