using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHud : MonoBehaviour {


    Slider healthSlider;


    private void Start()
    {
        healthSlider = GetComponent<Slider>();
    }
    private void Update()
    {

        healthSlider.value = PlayerData.PlayerDataValue.health;
       
       
    }


}
