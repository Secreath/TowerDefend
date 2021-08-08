using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using level;
using mapThing;
using tower;
using ui;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform uiCanvas;
    
    

    private GameState _gameState;
    public static GameState gameState => Instance._gameState;

    private int curLevelIndex;
    public int CurLevelIndex => curLevelIndex;
    
    [HideInInspector]
    public List<TowerType> choosedTowerList;
    [HideInInspector]
    public Dictionary<TowerType,int> towerCount;
    
    public static Dictionary<TowerType, TileType> TypeDic;
    
    private Dictionary<TowerType, GameObject> chooseUiDic;
    private Dictionary<TowerType, GameObject> upgradeUiDic;
    
    private Dictionary<Vector3Int, Point> pointDic = new Dictionary<Vector3Int, Point>();
    public List<Point> pointList = new List<Point>();
    
    public static Dictionary<int, List<Point>> PointsDic;

    public List<Point> startPoints;
    public List<Point> finishPoints;
    
    private void Start()
    {
        uiCanvas.gameObject.AddComponent<UiManager>();
    }
    
    public void Init()
    {
        InitPointLine();
        InitTypeDic();
        InitStartAndFinishPoints();
        
        EventCenter.GetInstance().EventTrigger("GameManagerInit");     
        UiManager.Instance.Init();
        LevelMgr.Instance.Init();
        
    }

    public void InitPointLine()
    {
        PointsDic = new Dictionary<int, List<Point>>();
        List<List<Transform>> roadList = Road.Instance.roadList;
        for (int j = 0; j < roadList.Count; j++)
        {
            List<Point> pointList = new List<Point>();
            List<Transform> posList = roadList[j];
            
            for (int i = 0; i < posList.Count; i++)
            {
                pointList.Add(GetPointByPos(posList[i].position));
            }
            PointsDic.Add(j,pointList);
        }
    }
    
    public void InitTypeDic()
    {
        towerCount = new Dictionary<TowerType, int>();
        TypeDic = new Dictionary<TowerType, TileType>();
        
        for (int i = 0; i < choosedTowerList.Count; i++)
        {
            towerCount.Add(choosedTowerList[i],0);
            TypeDic.Add(choosedTowerList[i],AssestMgr.Instance.GetTower(choosedTowerList[i]).tileType);
        }
    }
    
    public void InitStartAndFinishPoints()
    {
        startPoints = new List<Point>();
        finishPoints = new List<Point>();

        for (int i = 0;PointsDic.ContainsKey(i) ; i++)
        {
            Point start = PointsDic[i].First();
            Point finish = PointsDic[i].Last();
            if(!startPoints.Contains(start))
                startPoints.Add(start);
            if(!finishPoints.Contains(finish))
                finishPoints.Add(finish);
        }
    }
    
    public static List<Point> GetPointList(int roadID)
    {
        if(PointsDic.ContainsKey(roadID))
            return PointsDic[roadID];
        
        Debug.LogError($"map dont contains this rode {roadID}");
        return new List<Point>();
    }
    
    public static Point GetPointByPos(Vector3Int pos)
    {
        if (Instance.pointDic.ContainsKey(pos))
            return Instance.pointDic[pos];
        
        Debug.LogError($"map dont contains this point {pos}");
        return null;
    }
    
    public static Point GetPointByPos(Vector3 pos)
    {
        Vector3Int pointPos = VTool.ToPointPos(pos);
        return GetPointByPos(pointPos);
    }
    
    public static List<Point> GetPointsByPos(List<Vector2> listVec2)
    {
        List<Point> points = new List<Point>();
        for (int i = 0; i < listVec2.Count; i++)
        {
            Vector3Int pointPos = VTool.ToPointPos(listVec2[i]);
            points.Add(GetPointByPos(pointPos));
        }

        return points;
    }
    
    public bool HadThisPoint(Vector3Int pos)
    {
        return pointDic.ContainsKey(pos);
    }
    
    public void AddTileDic(Point point)
    {
        pointList.Add(point);
        pointDic.Add(point.Pos,point);
    }

    public static void ChangeGameState(GameState gameState)
    {
        Instance._gameState = gameState;
    }

    public void RegisterTower(GameObject tower,TowerType type)
    {
        tower.name = $"{type}{towerCount[type]}";
        towerCount[type]++;
    }
    public static JState GetJState(Point point)
    {
        
        if ((point.HadTower && gameState != GameState.PickTower) || (point.towerType == TowerType.UpGrade && point.BaseTower.state == TowerState.UpgradComplete))
        {
            return JState.PickUp;
        }
        else if (gameState == GameState.PickTower && (!point.HadTower || (point.HadTower && point.towerType == TowerType.UpGrade)))
        {
            return JState.PutDown;
        }

        return JState.ControllUi;
    }
}
