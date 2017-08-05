using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject boss;

    public float spawnTime = 3f;
    public float spawnTime2 = 6f;
    public float spawnTime3 = 15f;
    public Transform[] spawnPoints;



    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        InvokeRepeating("Spawn2", spawnTime2, spawnTime2);
        InvokeRepeating("Spawn3", spawnTime3, spawnTime3);
    }

    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

    }
    void Spawn2()
    {
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(enemy2, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

    }
    void Spawn3()
    {
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(boss, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

    }
}
