using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
    Map map;
    public static Transform player;
    public float moveSpeed;
    private void Start()
    {
        map = GetComponent<Map>();
    }
    private void Update()
    {
        if(map.isDraw && player != null && map.go.Count > 0)
        {
            Move();
        }
    }

    public void Move()
    {
        Vector3 movement = Vector3.MoveTowards(player.position,new Vector3(map.go[map.go.Count-1].x, 0, -map.go[map.go.Count-1].y),moveSpeed * Time.deltaTime);
        player.position = movement; 
        if(Vector3.Distance(player.position,new Vector3(map.go[map.go.Count-1].x, 0, -map.go[map.go.Count-1].y)) <= 0.1f)
        {
            map.go.RemoveAt(map.go.Count-1);
        }
        if(map.go.Count == 0)
        {
            map.isLand = true;
            map.isFind = false;
        }
    }
}
