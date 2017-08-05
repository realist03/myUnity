using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignChange : MonoBehaviour
{
    public Material sign;
    Color black = new Color(0, 0, 0, 0);

    void ChangeLight()
    {
        sign.color = black;
    }
}
