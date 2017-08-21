using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public Renderer enemy;

    void Start ()
    {
        InitMaterial();
    }
    protected override void Update()
    {
        base.Update();
    }


    public void InitMaterial()
    {
        if (curColor == chaColor.Blue)
        {
            enemy.sharedMaterial = blue;
            shellM.sharedMaterial = blue;
        }
        else
        {
            enemy.sharedMaterial = red;
            shellM.sharedMaterial = red;
        }
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
