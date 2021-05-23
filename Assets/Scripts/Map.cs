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
        private Tilemap background;
        private Tilemap road;

        private List<Vector3Int> pathList;
        private List<Vector3Int> bgList;
        
        
        
        void Start()
        {
            background = transform.Find("background").GetComponent<Tilemap>();
            road = transform.Find("road").GetComponent<Tilemap>();
        }

        public void Init(int curLevelIndex)
        {
            GetPathList();
            GetBgList();
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
        
        private void GetBgList()
        {
            bgList = new List<Vector3Int>();

            for (int i = background.origin.y; i < background.origin.y + background.size.y; i++)
            {
                for (int j = background.origin.x; j < background.origin.x + background.size.x; j++)
                {
                    Vector3Int pos = new Vector3Int(j,i,0);
                    
                    if (background.HasTile(pos) && !pathList.Contains(pos))
                    {
                        PlaceTile(TileType.BackGround, pos);
                        bgList.Add(pos);
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
