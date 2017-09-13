using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPost : Printer
{
    public CollisionCondition condition;
    public float conditionTime = 1;
    public LayerMask layers;

    public Transform castPoint;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
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

        shellCurColor = actor.state.curColor;

        prints[0] = actor.transform.Find("Splash 1").GetComponent<Decal>();
        VFX = actor.transform.Find("rollerVFX").GetComponent<ParticleSystem>();

        VFX_l = Instantiate(VFX, VFX_L.position, Quaternion.Euler(-30, -90, 0), VFX_L);
        VFX_r = Instantiate(VFX, VFX_R.position,Quaternion.Euler(-30,90,0),VFX_R);
    }
    void FixedUpdate()
    {
        CastCollision(Time.fixedDeltaTime);
        if(actor.state.isMove)
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
                actor.CmdAddV(intPos);
                Print(Mapping.mapV[(Mapping.mapV.Count - 1)], collision.rotation, collision.surface, collision.layer);
            }
        }
        else
        {
            actor.CmdRemoveMapInfo(new Vector2(posX, posZ));
            actor.CmdSetMapInfo(new Vector2(posX, posZ), (int)shellCurColor);
            actor.CmdAddV(intPos);
            Print(Mapping.mapV[(Mapping.mapV.Count - 1)], collision.rotation, collision.surface, collision.layer);
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