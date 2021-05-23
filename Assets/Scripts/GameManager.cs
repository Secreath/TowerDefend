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

    public static Dictionary<TowerType, TileType> TypeDic;
    
    private Dictionary<TowerType, GameObject> chooseUiDic;
    private Dictionary<TowerType, GameObject> upgradeUiDic;
    
    private Dictionary<Vector3Int, Point> pointDic = new Dictionary<Vector3Int, Point>();
    public static Dictionary<int, List<Point>> PointsDic;

    public static List<Point> startPoints;
    public static List<Point> finishPoints;
    
    private void Start()
    {
        uiCanvas.gameObject.AddComponent<UiManager>();
        gameObject.AddComponent<AssestMgr>();
        
        
    }
    
    public void Init()
    {
        InitPointLine();
        InitTypeDic();
        InitStartAndFinishPoints();
        
        EventCenter.GetInstance().EventTrigger("GameManagerInit");     
        UiManager.Instance.Init();
        
    }

    public void InitStartAndFinishPoints()
    {
        startPoints = new List<Point>();
        finishPoints = new List<Point>();

        List<Transform> startTrans = TDRoad.Instance.startTrans;
        List<Transform> finishTrans = TDRoad.Instance.finishTrans;
        
        for (int i = 0; i < startTrans.Count; i++)
        {
            startPoints.Add(GetPointByPos(startTrans[i].position));
        }
        
        for (int i = 0; i < finishTrans.Count; i++)
        {
            finishPoints.Add(GetPointByPos(finishTrans[i].position));
        }
    }
    public void InitPointLine()
    {
        PointsDic = new Dictionary<int, List<Point>>();
        for (int j = 0; j < TDRoad.Instance.pathLines.Count; j++)
        {
            List<Point> pointList = new List<Point>();
            List<Transform> posList = TDRoad.GetPathList(j);
            
            for (int i = 0; i < posList.Count; i++)
            {
                pointList.Add(GetPointByPos(posList[i].position));
            }
            PointsDic.Add(j,pointList);
        }
    }

    public void InitTypeDic()
    {
        TypeDic = new Dictionary<TowerType, TileType>();
        for (int i = 0; i < choosedTowerList.Count; i++)
        {
            TypeDic.Add(choosedTowerList[i],AssestMgr.Instance.GetTower(choosedTowerList[i]).tileType);
        }
    }
    
    public static List<Point> GetPointList(int roadID)
    {
        if(PointsDic.ContainsKey(roadID))
            return PointsDic[roadID];
        return new List<Point>();
    }
    
    public static Point GetPointByPos(Vector3Int pos)
    {
        if (!Instance.pointDic.ContainsKey(pos))
            return null;

        return Instance.pointDic[pos];
    }
    
    public static Point GetPointByPos(Vector3 pos)
    {
        Vector3Int pointPos = VTool.ToPointPos(pos);
        return GetPointByPos(pointPos);
    }
    
    public bool HadThisPoint(Vector3Int pos)
    {
        return pointDic.ContainsKey(pos);
    }
    
    public void AddTileDic(Point point)
    {
        pointDic.Add(point.Pos,point);
    }

    public static void ChangeGameState(GameState gameState)
    {
        Instance._gameState = gameState;
    }

    public static JState GetJState(Point point)
    {
        
        if (point.HadTower && gameState != GameState.PickTower && point.type != TowerType.UpGrade)
        {
            return JState.PickUp;
        }
        else if (gameState == GameState.PickTower && (!point.HadTower || (point.HadTower && point.type == TowerType.UpGrade)))
        {
            return JState.PutDown;
        }

        return JState.ControllUi;
    }
}
