using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }


    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }
        
    public static bool operator ==(Point a, Point b)
    {
        if (a.X == b.X && a.Y == b.Y)
            return true;
        
        return false;
    }
}

public enum TileType
{
    Rode,
    Build,
    BackGround
}
