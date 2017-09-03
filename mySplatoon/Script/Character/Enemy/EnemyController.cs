using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyCharacter enemy;
    Transform player;
    UnityEngine.AI.NavMeshAgent nav;
    float attackTimer;

    void Awake()
    {
        enemy = GetComponent<EnemyCharacter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

	
	void FixedUpdate ()
    {
        nav.SetDestination(player.position);

        attackTimer += Time.deltaTime;
        if(attackTimer>=2)
        {
            enemy.Shoot();
            attackTimer = 0;
        }
    }
}
