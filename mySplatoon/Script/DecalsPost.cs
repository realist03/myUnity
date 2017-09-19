using System.Collections;
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

    public AudioSource post;
    public Actor actor;
    Vector3 intPos = new Vector3(0,0,0);

    Actor.eColor shellCurColor = Actor.eColor.None;

    void Start()
    {

        //Grab Particle System
        partSystem = GetComponent<ParticleSystem>();

        actor = GetComponentInParent<Actor>();

        shellCurColor = actor.state.curColor;

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

                    Debug.Log("被打中" + hit.collider.name);

                    actor.AddFloorPost(position);

                    if (hit.collider.gameObject == actor.gameObject)
                    {
                        i++;
                        continue;
                    }

                    if (hit.collider.tag == "Player")
                    {
                        var get = hit.collider.gameObject.GetComponentInParent<Actor>();

                        if (shellCurColor != get.state.curColor)
                        {
                            actor.AddFloorPost(position);
                            actor.AddPunchEffect(position);
                            get.TakeDamage(actor,get, normal);
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

                    intPos.x = posX + 0.5f;
                    intPos.y = posY;
                    intPos.z = posZ + 0.56f;

                    if (Mapping.map.ContainsKey(new Vector2(posX, posZ)))
                    {
                        if (Mapping.map[new Vector2(posX, posZ)] == shellCurColor)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            actor.CmdRemoveMapInfo(new Vector2(posX, posZ));
                            actor.CmdSetMapInfo(new Vector2(posX, posZ), (int)shellCurColor);
                            actor.CmdAddV(intPos);
                            if (!post.isPlaying)
                                post.Play();
                            if (Mapping.mapV != null && Mapping.mapV.Count != 0)
                            {
                                actor.data.points+= 5;
                                if (actor.data.power < actor.data.powerMax)
                                    actor.data.power++;
                                Print(intPos, Quaternion.LookRotation(-normal, rot), surface, hit.collider.gameObject.layer);
                            }
                        }
                    }
                    else
                    {
                        actor.CmdSetMapInfo(new Vector2(posX, posZ), (int)actor.state.curColor);
                        actor.CmdAddV(intPos);
                        if (!post.isPlaying)
                            post.Play();
                        if (Mapping.mapV != null&& Mapping.mapV.Count!=0)
                        {
                            actor.data.points += 5;
                            if(actor.data.power < actor.data.powerMax)
                                actor.data.power++;
                            Print(intPos, Quaternion.LookRotation(-normal, rot), surface, hit.collider.gameObject.layer);
                        }
                    }
                }
                i++;
            }
        }
    }
}