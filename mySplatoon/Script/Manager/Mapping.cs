using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public class Mapping : MonoBehaviour
{
    public static Dictionary<Vector2,Character.chaColor> painted = new Dictionary<Vector2, Character.chaColor>();
}
