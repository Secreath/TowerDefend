using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tile;

    public Dictionary<Point, TileScript> tileDic;

    private Point blueSpawn;
    private Point redSpawn;

    public GameObject bluePortal;
    private GameObject redPortal;
    
    private string[] mapData;
    public float TileSize
    {
        get
        {
            return tile[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }

    void Start()
    {
        CreateLevel();
    }

    public void SwapInt<T>(ref T a,ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }
    
    private void CreateLevel()
    {
        Vector3 offSet = new Vector3(TileSize/2,-TileSize/2);   
        Vector3 wordStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        
        mapData = ReadLevelText();
        int mapY = mapData.Length;
        int mapX = mapData[0].Length;
        
        tileDic = new Dictionary<Point, TileScript>();
        
        

        Vector3 maxTile = default;
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
               PlaceTile(x,y,newTiles[x].ToString(),offSet,wordStart);
            }
        }
        
        SpawnProtal();
    }
    
    private void PlaceTile(int x,int y,string typeChar,Vector3 offSet,Vector3 worldStart)
    {
        int type = int.Parse(typeChar);
        GameObject newTile = Instantiate(tile[type]);
        TileScript tileScript = newTile.GetComponent<TileScript>();
        
        Vector3 tilepos = new Vector3(TileSize * x + worldStart.x, -TileSize * y + worldStart.y, 0) + offSet;
        
        tileScript.SetUp(new Point(x, y),tilepos,transform);
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load<TextAsset>("Level");
        string data = bindData.text.Replace(Environment.NewLine,string.Empty);
        return data.Split(',');
    }


    private void SpawnProtal()
    {
        blueSpawn = new Point(0,0);

        Instantiate(bluePortal, tileDic[blueSpawn].CenterPos, Quaternion.identity);
    }
}

























