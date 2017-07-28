using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    HitscanWeapon weapon;
    public Slider HPSlider;
    public Image healthFillImage;                           
    public Color healthColorFull = Color.red;
    public Color HealthColorNull = Color.black;

    public Text hpText;

    public Text countText;
    public Text totalCount;

    public Slider reFill;

    private void Awake()
    {
        weapon = FindObjectOfType<HitscanWeapon>();
        GameplayStatics.LocalPlayer.health.AddChangedListener(ChangeHP);
        weapon.bulletsCount.AddChangedListener(ChangeCount);
        weapon.totalCount.AddChangedListener(ReFillCount);
    }

    void ChangeHP()
    {
        var playerHP = GameplayStatics.LocalPlayer.health.Get();
        HPSlider.value = playerHP;
        hpText.text = "HP:" + playerHP.ToString();
        healthFillImage.color = Color.Lerp(HealthColorNull, healthColorFull, playerHP / 100);
    }

    void ChangeCount()
    {
        var currentCount = weapon.bulletsCount.Get();
        countText.text = currentCount.ToString();
    }

    void ReFillCount()
    {
        var totalCurrentCount = weapon.totalCount.Get();
        totalCount.text = totalCurrentCount.ToString();
        if(GameplayStatics.LocalPlayer.reFillStart.Get() == true)
        {
            reFill.gameObject.SetActive(true);
            reFill.value = GameplayStatics.LocalPlayer.reFillRate.Get();
        }
        if (GameplayStatics.LocalPlayer.reFillStart.Get() == false)
        {
            reFill.gameObject.SetActive(false);
        }

    }


    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
