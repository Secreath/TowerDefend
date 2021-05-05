using System;
using System.Collections;
using System.Collections.Generic;
using mapThing;
using tower;
using UnityEngine;


public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Vector3Int Pos
    {
        get{ return new Vector3Int(X,Y,0);}
    }
    
    public Vector3 CenterPos
    {
        get{ return new Vector3(X + 0.5f,Y + 0.5f);}
    }

    public TowerType type
    {
        get
        {
            if (baseTower == null)
                return TowerType.End;

            return baseTower.tower.type;
        }
    }

    public bool HadTower;
    
    private BaseTile tile;
    public BaseTile Tile
    {
        get { return tile; }
    }

    private BaseTower baseTower;

    public BaseTower BaseTower
    {
        get { return baseTower; }
    }
    
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
        HadTower = false;
        tile = default;
        baseTower = default;
        GameManager.Instance.AddTileDic(this);
    }
    
    public void SetTile(BaseTile tile)
    {
        this.tile = tile;
    }

    public void SetTower(BaseTower tower)
    {
        HadTower = true;
        this.baseTower = tower;
    }

    public void RemoveTower()
    {
        HadTower = false;
        this.baseTower = default;
    }
    
    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }
        
    public static bool operator ==(Point a, Point b)
    {
        if (a == null || b == null)
            return false;
        
        if (a.X == b.X && a.Y == b.Y)
            return true;
        
        return false;
    }
}

