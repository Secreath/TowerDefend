using System;
using System.Collections;
using System.Collections.Generic;
using tower;
using ui;
using UnityEngine;

namespace mapThing
{
    public enum TileType
    {
        road,
        Build,
        BackGround,
        All
    }
    
    public class BaseTile
    {
        private Map map;
        private bool hadBuild;
        public bool HadBuild
        {
            get { return hadBuild; }
        }
        
        private Point point;

        public Point Point
        {
            get { return point; }
        }
       
        private TileType tileType;
        public TileType TileType
        {
             get { return tileType;}
        }

        private GameObject towerObj;

        public GameObject TowerObj
        {
            get { return towerObj;}
        }
        
        private Vector2 size;

        public BaseTile(Map map,Point gridPos,TileType type)
        {
            this.map = map;
            this.point = gridPos;
            this.tileType = type;
        }


        public void BuildTower(TowerType towerType)
        {
            if(TileType != TileType.Build || hadBuild)
                return;
            
            hadBuild = true;
            towerObj.GetComponent<SpriteRenderer>().sortingOrder = point.Y;

        }
    }
}
