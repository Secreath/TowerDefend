//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class LevelManager : Singleton<LevelManager>
//{
//    [SerializeField]
//    private GameObject[] tile;
//
//    public Dictionary<Point, BaseTile> tileDic;
//
//    private Point blueSpawn;
//    private Point redSpawn;
//
//    public GameObject bluePortal;
//    private GameObject redPortal;
//    
//    private string[] mapData;
//    public float TileSize
//    {
//        get
//        {
//            return tile[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
//        }
//    }
//
//    void Start()
//    {
//        CreateLevelByText();
//    }
//
//    #region CreateByText
//    private void SpawnProtal()
//    {
//        blueSpawn = new Point(0,0);
//
//        Instantiate(bluePortal, tileDic[blueSpawn].CenterPos, Quaternion.identity);
//    }
//    
//    private string[] ReadLevelText()
//    {
//        TextAsset bindData = Resources.Load<TextAsset>("Level");
//        string data = bindData.text.Replace(Environment.NewLine,string.Empty);
//        return data.Split(',');
//    }
//    
//    private void CreateLevelByText()
//    {
//        Vector3 offSet = new Vector3(TileSize/2,-TileSize/2);   
//        Vector3 wordStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
//        
//        mapData = ReadLevelText();
//        int mapY = mapData.Length;
//        int mapX = mapData[0].Length;
//        
//        tileDic = new Dictionary<Point, BaseTile>();
//        
//        
//
//        Vector3 maxTile = default;
//        for (int y = 0; y < mapY; y++)
//        {
//            char[] newTiles = mapData[y].ToCharArray();
//            for (int x = 0; x < mapX; x++)
//            {
//               PlaceTile(x,y,newTiles[x].ToString(),offSet,wordStart);
//            }
//        }
//        
//        SpawnProtal();
//    }
//
//    #endregion
//    
//    private void PlaceTile(int x,int y,string typeChar,Vector3 offSet,Vector3 worldStart)
//    {
//        int type = int.Parse(typeChar);
//        GameObject newTile = Instantiate(tile[type]);
//        BaseTile baseTile = newTile.GetComponent<BaseTile>();
//        
//        Vector3 tilepos = new Vector3(TileSize * x + worldStart.x, -TileSize * y + worldStart.y, 0) + offSet;
//        
////        baseTile.SetUp(new Point(x, y),tilepos,transform);
//    }
//
//    
//
//
//   
//}
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
