using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Post : MonoBehaviour
{
    public GameObject hitPost;
    public ParticleSystem postFX;

    private PlayerHealth playerHealth;

    Character character;

    public bool isPostBlue;

    private void Start()
    {
        character = FindObjectOfType<Character>();
        isPostBlue = character.isBlue;
        playerHealth = GetComponent<PlayerHealth>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hitInfo;

        var FXdir = Quaternion.Euler(collision.transform.rotation.x, -collision.transform.rotation.y, collision.transform.rotation.z);
        if (collision.gameObject.tag != "Shell" && collision.gameObject.tag != "Player" && Physics.Raycast(transform.position, Vector3.forward, out hitInfo, 1f))
        {
            //gameObject.SetActive(false);

            GameObject hit;
            Quaternion angles = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            //ParticleSystem fireFX;
            if (collision.gameObject.tag == "Floor")
            {
                hit = Instantiate(hitPost, hitInfo.point, Quaternion.Euler(90, 0, 0));
                hit.gameObject.SetActive(true);
                hit.transform.parent = collision.transform;
                Destroy(gameObject, 1);
                Destroy(hit, 10);
                return;
            }
            if (collision.gameObject.tag == "ZomBear" || collision.gameObject.tag == "ZomBunny")
            {
                hit = Instantiate(hitPost, hitInfo.point, Quaternion.identity);
                hit.gameObject.SetActive(true);
                hit.transform.parent = collision.transform;
                Destroy(gameObject);
                return;
            }
            else
            {
                hit = Instantiate(hitPost, hitInfo.point, angles);
                hit.gameObject.SetActive(true);
                hit.transform.parent = collision.transform;

                //fireFX = Instantiate(postFX, hitInfo.point, FXdir);
                //fireFX.gameObject.SetActive(true);

                //Destroy(fireFX, 3);
                Destroy(hit, 2);
                return;
            }
        }
    }
}