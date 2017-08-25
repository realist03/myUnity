﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* The CollisionPrinter Component. Prints a projection under set conditions related to the collision of the object attached to this printer.
*/
[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class DecalsPost : Printer
{
    /**
    * Defines the orientation of the projection relative to the surface of the collision. Velocity will orient the projection as if its up is the direction the collision object is moving in. Random will orient the projection as if its up is random.
    */
    public RotationSource rotationSource;

    private ParticleSystem partSystem;
    private float maxparticleCollisionSize;
    private List<ParticleCollisionEvent> collisionEvents;

    public Actor actor;

    Actor.eColor shellCurColor = Actor.eColor.None;

    void Start()
    {
        //Grab Particle System
        partSystem = GetComponent<ParticleSystem>();

        actor = GetComponentInParent<Actor>();

        shellCurColor = actor.curColor;

        if (Application.isPlaying)
        {
            //Initialize collision list
            collisionEvents = new List<ParticleCollisionEvent>();
        }
        //InitColor();
    }
    void Update()
    {
        // Log Setup Warnings
        if (partSystem.collision.enabled != true)
        {
            Debug.LogWarning("Particle system collisions must be enabled for the particle system to print decals");
        }
        else if (partSystem.collision.sendCollisionMessages != true)
        {
            Debug.LogWarning("Particle system must send collision messages for the particle system to print decals. This option can be enabled under the collisions menu.");
        }
    }
    
    void OnParticleCollision(GameObject other)
    {
        if (Application.isPlaying)
        {
            int numCollisionEvents = partSystem.GetCollisionEvents(other, collisionEvents);

            int i = 0;
            while (i < numCollisionEvents)
            {
                //Grab our collision data
                Vector3 position = collisionEvents[i].intersection;
                Vector3 normal = collisionEvents[i].normal;
                Transform surface = other.transform;

                //Create our layermask
                int layerMask = 1 << other.layer;

                //Calculate final position and surface normal
                RaycastHit hit;
                if (Physics.Raycast(position, -normal, out hit, Mathf.Infinity, layerMask))
                {
                    position = hit.point;
                    normal = hit.normal;
                    surface = hit.collider.transform;


                    Debug.Log("被打中"+ hit.collider.name);
                    if (hit.collider.tag == "Enemy" || (hit.collider.tag == "Player"))
                    {
                        var get = hit.collider.gameObject.GetComponentInParent<Actor>();

                        if(shellCurColor != get.curColor)
                        {
                            //get.TakeDamage(20);
                            Debug.Log("被打中");
                        }
                    }

                    //Calculate our rotation
                    Vector3 rot;
                    if (rotationSource == RotationSource.Velocity && collisionEvents[i].velocity != Vector3.zero) rot = collisionEvents[i].velocity.normalized;
                    else if (rotationSource == RotationSource.Random) rot = Random.insideUnitSphere.normalized;
                    else rot = Vector3.up;


                    var posX = Mathf.FloorToInt(position.x);
                    var posY = Mathf.FloorToInt(position.y);
                    var posZ = Mathf.FloorToInt(position.z);

                    Vector3 intPos = new Vector3(posX+0.5f, position.y, posZ+0.56f);

                    if (Mapping.map.ContainsKey(new Vector2(posX, posZ)))
                    {
                        if (Mapping.map[new Vector2(posX, posZ)] == shellCurColor)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            Mapping.map[new Vector2(posX, posZ)] = shellCurColor;
                            Print(intPos, Quaternion.LookRotation(-normal, rot), surface, hit.collider.gameObject.layer);
                        }
                    }
                    else
                    {
                        Mapping.map.Add(new Vector2(posX, posZ), actor.curColor);
                        Print(intPos, Quaternion.LookRotation(-normal, rot), surface, hit.collider.gameObject.layer);
                        //Debug.Log("dic:" + posX + "," + posZ);
                    }
                }
                i++;
            }
        }
    }
}