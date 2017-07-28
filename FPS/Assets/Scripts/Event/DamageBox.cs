using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    PlayerCharacter player;
    public float boxDamage = 20;
    // Use this for initialization
    private void Awake()
    {
        player = FindObjectOfType<PlayerCharacter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(boxDamage);
        }
    }
}
