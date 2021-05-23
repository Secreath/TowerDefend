using System;
using System.Collections;
using System.Collections.Generic;
using mapThing;
using tower;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;


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

    public TileType tileType => tile.TileType;
    
    public TowerType type
    {
        get
        {
            if (baseTower == null)
                return TowerType.End;

            return baseTower.type;
        }
    }

    public bool HadTower => baseTower != default;
    
    private BaseTile tile;
    public BaseTile Tile => tile;

    private BaseTower baseTower;

    public BaseTower BaseTower => baseTower;
    
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
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
        if (tileType == tower.requireTile)
        {
            this.baseTower = tower;
            tower.Init();
        }
        else
        {
            Debug.Log("TileTypeError");
        }
    }

    public void RemoveTower()
    {
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

