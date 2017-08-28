using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Pos
{
    int x;
    int y;

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Pos other)
    {
        if (x == other.x && y == other.y)
        {
            return true;
        }
        else
            return false;
    }
}
public class Mapping : NetworkBehaviour
{
    public static Dictionary<Vector2,Character.chaColor> painted = new Dictionary<Vector2, Character.chaColor>();

    public static Dictionary<Vector2, Actor.eColor> map = new Dictionary<Vector2, Actor.eColor>();
}
