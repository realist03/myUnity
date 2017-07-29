using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBox : MonoBehaviour
{
    HitscanWeapon weapon;
    public int supplementCount = 20;

    private void Awake()
    {
        weapon = FindObjectOfType<HitscanWeapon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            weapon.SupplementBullets(supplementCount); 
        }
    }

}
