using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* The RayCollisionPrinter Component. Given a transform, it projects a ray that starts at the transforms position and casts in the transforms forward direction. It then prints a projection under set conditions relating to that raycast.
*/
public class CollisionPost : Printer
{
    /**
    * Defines the condition on which a projection is printed. Enter will print whenever a ray-collision occurs. Delay will print the conditionTime seconds after a ray-collision occurs. Constant will print every fixed update during a ray-collision. Exit will print upon exiting a ray-collision.
    */
    public CollisionCondition condition;
    /**
    * If the collision condition is set to delay, the conditionTime determines the length of that delay.
    */
    public float conditionTime = 1;

    /**
    * The layers that, when hit by a ray with, cause a print.
    */
    public LayerMask layers;

    //Cast Properties
    /**
    * The transform that defines the collision ray. If left null will default to the attached transform. The transforms position will be used as a base for the rays starting position & it's forward direction will be used as a base for the rays direction.
    */
    public Transform castPoint;
    /**
    * The position offset is applied to the castPoint to get the starting point of the collision ray. This essentially allows you to offset the rays starting position.
    */
    public Vector3 positionOffset;
    /**
    * The rotation offset is applied to the castPoint transforms forward direction to get the direction of the collision ray. This essentially allows you to offset the rays direction.
    */
    public Vector3 rotationOffset;
    /**
    * The length of the ray thats cast.
    */
    public float castLength = 1;

    public RotationSource rotationSource;
    private List<ParticleCollisionEvent> collisionEvents;

    public Actor actor;

    Actor.eColor shellCurColor = Actor.eColor.None;

    public ParticleSystem VFX;
    public ParticleSystem VFX_l;
    public ParticleSystem VFX_r;

    public Transform VFX_L;
    public Transform VFX_R;

    private void Start()
    {
        actor = GetComponentInParent<Actor>();

        shellCurColor = actor.curColor;

        prints[0] = actor.transform.Find("Splash 1").GetComponent<Decal>();
        VFX = actor.transform.Find("rollerVFX").GetComponent<ParticleSystem>();

        VFX_l = Instantiate(VFX, VFX_L.position, Quaternion.Euler(-30, -90, 0), VFX_L);
        VFX_r = Instantiate(VFX, VFX_R.position,Quaternion.Euler(-30,90,0),VFX_R);
    }
    void FixedUpdate()
    {
        CastCollision(Time.fixedDeltaTime);
        if(actor.isMove)
        {
            VFX_l.gameObject.SetActive(true);
            VFX_r.gameObject.SetActive(true);
        }
        else
        {
            VFX_l.gameObject.SetActive(false);
            VFX_r.gameObject.SetActive(false);
        }
    }

    #region Collision
    CollisionData collision;

    float timeElapsed;
    bool delayPrinted;

    private void CastCollision(float deltaTime)
    {
        Vector3 rot;
        if (rotationSource == RotationSource.Velocity && GetComponent<Rigidbody>().velocity != Vector3.zero) rot = GetComponent<Rigidbody>().velocity.normalized;
        else if (rotationSource == RotationSource.Random) rot = Random.insideUnitSphere.normalized;
        else rot = Vector3.up;

        //Calculate Target Position and Rotation
        Transform origin = (castPoint != null) ? castPoint : transform;
        Quaternion Rotation = origin.rotation * Quaternion.Euler(rotationOffset);
        Vector3 Position = origin.position + (Rotation * positionOffset);

        //Check for collision
        RaycastHit hit;
        Ray ray = new Ray(Position, Rotation * Vector3.forward);
        if (Physics.Raycast(transform.position,-Vector3.up, out hit, castLength, layers.value))
        {
            //Calculate Data
            collision = new CollisionData(hit.point, Quaternion.LookRotation(-hit.normal, rot), hit.transform, hit.collider.gameObject.layer);

            //If Condition is Constant
            if (condition == CollisionCondition.Constant)
            {
                PrintCollision(collision);
            }

            //If Condition is Enter
            if (timeElapsed == 0)
            {
                if (condition == CollisionCondition.Enter)
                {
                    PrintCollision(collision);
                }
            }

            //Update collision time
            timeElapsed += deltaTime;

            //If Condition is Delayed and delay has passed
            if (condition == CollisionCondition.Delay && timeElapsed >= conditionTime && !delayPrinted)
            {
                PrintCollision(collision);
                delayPrinted = true;
            }
        }
        else
        {
            //If condition is Exit || Delayed and premature
            if (timeElapsed > 0 && (condition == CollisionCondition.Exit || (condition == CollisionCondition.Delay && timeElapsed < conditionTime)))
            {
                PrintCollision(collision);
            }

            //Set up our collision
            timeElapsed = 0;
            delayPrinted = false;
        }
    }
    #endregion

    private void PrintCollision(CollisionData collision)
    {
        var position = collision.position;
        var posX = Mathf.FloorToInt(position.x);
        var posY = Mathf.FloorToInt(position.y);
        var posZ = Mathf.FloorToInt(position.z);

        Vector3 intPos = new Vector3(posX + 0.5f, position.y, posZ + 0.56f);

        if (Mapping.map.ContainsKey(new Vector2(posX, posZ)))
        {
            if (Mapping.map[new Vector2(posX, posZ)] == shellCurColor)
            {
            }
            else
            {
                actor.CmdRemoveMapInfo(new Vector2(posX, posZ));
                actor.CmdSetMapInfo(new Vector2(posX, posZ), (int)shellCurColor);
                Print(intPos, collision.rotation, collision.surface, collision.layer);
            }
        }
        else
        {
            actor.CmdSetMapInfo(new Vector2(posX, posZ), (int)actor.curColor);
            Print(intPos, collision.rotation, collision.surface, collision.layer);
        }

    }

    void OnDrawGizmos()
    {
        Transform origin = (castPoint != null) ? castPoint : transform;
        Quaternion Rotation = origin.rotation * Quaternion.Euler(rotationOffset);
        Vector3 Position = origin.position + (Rotation * positionOffset);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(Position, Rotation * Vector3.up * 0.4f);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(Position, Rotation * Vector3.forward * castLength);
    }
}

internal struct eCollisionData
{
    public Vector3 position;
    public Quaternion rotation;
    public Transform surface;
    public int layer;

    public eCollisionData(Vector3 Position, Quaternion Rotation, Transform Surface, int Layer)
    {
        position = Position;
        rotation = Rotation;
        surface = Surface;
        layer = Layer;
    }
}