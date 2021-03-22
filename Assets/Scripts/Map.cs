using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Tilemap background;
    public Tilemap path;

    private List<Vector3Int> pathList;

    private Vector3Int startPoint;
    private Vector3Int endPoint;
    void Start()
    {
        startPoint = Vector3Int.zero;
        endPoint = Vector3Int.zero;
        GetStartPointAndEndPoint();
        
        BoundsInt bounds = path.cellBounds;
        TileBase[] allTiles = path.GetTilesBlock(bounds);
        
        Debug.Log(startPoint + "  " + endPoint);
        GetPathList();
    }
    
    private void GetPathList()
    {
        BoundsInt bounds = path.cellBounds;
//        TileBase[] allTiles = path.GetTilesBlock(bounds);
        pathList = new List<Vector3Int>();

        for (int i = path.origin.y; i < path.origin.y + path.size.y; i++)
        {
            for (int j = path.origin.x; j < path.origin.x + path.size.x; j++)
            {
                Vector3Int pos = new Vector3Int(j,i,0);
                if (path.HasTile(pos))
                {
                    Debug.Log(pos);
                    pathList.Add(pos);
                }
            }
        }
    }
    private void GetStartPointAndEndPoint()
    {
        while (background.HasTile(startPoint))
        {
            startPoint += Vector3Int.down;
        }

        startPoint -= Vector3Int.down;
        
        while (background.HasTile(startPoint))
        {
            startPoint += Vector3Int.left;
        }

        startPoint -= Vector3Int.left;
        while (background.HasTile(endPoint))
        {
            endPoint += Vector3Int.up; 
        }
        endPoint -= Vector3Int.up;
        
        while (background.HasTile(endPoint))
        {
            endPoint += Vector3Int.right; 
        }
        endPoint -= Vector3Int.right;
    }
}
