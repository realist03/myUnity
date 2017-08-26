using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public Renderer playerModel;
    public Renderer InkFish;

    BoxCollider boxCollider;
    Rigidbody rigid;

    public int jumpHeight;

    public bool jumpInput;
    public bool Grounded
    {
        get { return Physics.Raycast(transform.position, -Vector3.up, boxCollider.bounds.extents.y * 1.2f); }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponentInChildren<BoxCollider>();
    }
    protected override void Update()
    {
        base.Update();

    }

    private void FixedUpdate()
    {
        Vector3 velocity = rigid.velocity;

        if (jumpInput && Grounded)
        {
            velocity.y = jumpHeight;
        }

        rigid.velocity = velocity;

    }

    public void TransColor()
    {
        if (curColor == chaColor.Blue)
        {
            curColor = chaColor.Red;
            playerModel.sharedMaterial = red;
            InkFish.sharedMaterial = red;
        }
        else
        {
            curColor = chaColor.Blue;
            playerModel.sharedMaterial = blue;
            InkFish.sharedMaterial = blue;
        }
    }

    protected override void Die()
    {
        base.Die();
        GameMode.isGameOver = true;
    }
}
