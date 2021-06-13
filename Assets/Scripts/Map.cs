using System;
using System.Collections;
using System.Collections.Generic;
using tower;
using ui;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace mapThing
{
    
    public class Map : Singleton<Map>
    {
        private Tilemap build;
        private Tilemap road;

        private List<Vector3Int> pathList;
        private List<Vector3Int> buildList;
        

        public void Init(int curLevelIndex)
        {
            build = transform.Find("background").GetComponent<Tilemap>();
            road = transform.Find("road").GetComponent<Tilemap>();
            GetPathList();
            GetBuildList();
            AssestMgr.Instance.Init(curLevelIndex);
        }
        
        private void GetPathList()
        {
            pathList = new List<Vector3Int>();

            for (int i = road.origin.y; i < road.origin.y + road.size.y; i++)
            {
                for (int j = road.origin.x; j < road.origin.x + road.size.x; j++)
                {
                    Vector3Int pos = new Vector3Int(j,i,0);
                    if (road.HasTile(pos))
                    {
                        PlaceTile(TileType.road, pos);
                        pathList.Add(pos);
                    }
                }
            }
        }
        
        private void GetBuildList()
        {
            buildList = new List<Vector3Int>();

            for (int i = build.origin.y; i < build.origin.y + build.size.y; i++)
            {
                for (int j = build.origin.x; j < build.origin.x + build.size.x; j++)
                {
                    Vector3Int pos = new Vector3Int(j,i,0);
                    
                    if (build.HasTile(pos) && !pathList.Contains(pos))
                    {
                        PlaceTile(TileType.Build, pos);
                        buildList.Add(pos);
                    }
                }
            }
        }
        
//        private void OnMouseOver()
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                UiManager.Instance.ShowTowerUi(GetMousePos.GetMousePosition());
//            }
//        }
        
        private void PlaceTile(TileType tileType,Vector3Int pos)
        {
            Point point = new Point(pos.x, pos.y);
            point.SetTile(new BaseTile(this,point,tileType));
            
        }
        
    }

}
