using System;
using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public List<int> choosedTowerId = new List<int>();

    private Dictionary<TowerType, GameObject> towerDic;
    private Dictionary<Vector3Int, Point> pointDic;
    
    private void Start()
    {
        pointDic = new Dictionary<Vector3Int, Point>();
        StartCoroutine(LoadAllTowerObj());
    }

    private IEnumerator LoadAllTowerObj()
    {
        towerDic = new Dictionary<TowerType, GameObject>();
        ResourceRequest rr;
        
        for (int i = 0; i < choosedTowerId.Count; i++)
        {
            TowerType towerType = (TowerType) choosedTowerId[i];
            rr = Resources.LoadAsync<GameObject>("Tower/" + towerType.ToString() + "Tower");
            yield return rr;
            towerDic.Add(towerType,rr.asset as GameObject);
        }
    }

    public GameObject GetTowerByType(TowerType towerType)
    {
        if (!towerDic.ContainsKey(towerType))
            return null;

        return towerDic[towerType];
    }
    
    public bool HadThisTowerType(TowerType towerType)
    {
        return towerDic.ContainsKey(towerType);
    }
    
    
    public Point GetPointByPos(Vector3Int pos)
    {
        if (!pointDic.ContainsKey(pos))
            return null;

        return pointDic[pos];
    }
    
    public bool HadThisPoint(Vector3Int pos)
    {
        return pointDic.ContainsKey(pos);
    }
    
    public void AddTileDic(Point point)
    {
        pointDic.Add(point.Pos,point);
    }
    
}
