using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    void Start ()
    {
        InitMaterial();
    }
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerShell")
        {
            var shellM = collision.gameObject.GetComponent<Renderer>();

            if(curColor == chaColor.Blue && shellM.sharedMaterial.name != "Blue")
            {
                TakeDamage(playerShellDamage);
            }
            else if(curColor == chaColor.Red && shellM.sharedMaterial.name != "Red")
            {
                TakeDamage(playerShellDamage);
            }
        }
    }
}
