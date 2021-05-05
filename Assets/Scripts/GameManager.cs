using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tower;
using ui;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform uiCanvas;
    public string chooseTowers;
    
    

    private GameState _gameState;
    public GameState gameState => _gameState;
    
    
    [HideInInspector]
    public List<TowerType> choosedTowerList;

    private Dictionary<TowerType, GameObject> chooseUiDic;
    private Dictionary<TowerType, GameObject> upgradeUiDic;
    
    private Dictionary<Vector3Int, Point> pointDic = new Dictionary<Vector3Int, Point>();
    
    
    private void Start()
    {
        uiCanvas.gameObject.AddComponent<UiManager>();
        gameObject.AddComponent<AssestMgr>();
        SeparateStrToTowerType();
        
    }

    private void SeparateStrToTowerType()
    {
        choosedTowerList = new List<TowerType>();
        string[] strs = chooseTowers.Split(',');
        for (int i = 0; i < strs.Length; i++)
        {
            if (int.TryParse(strs[i], out int type))
            {
                TowerType towerType = (TowerType) type;
                if(choosedTowerList.Contains(towerType))
                    continue;
                choosedTowerList.Add(towerType);
            }
        }
        AssestMgr.Instance.Init(choosedTowerList);
    }

    public void Init()
    {
        EventCenter.GetInstance().EventTrigger("GameManagerInit");     
        UiManager.Instance.Init();
    }
    
    public Point GetPointByPos(Vector3Int pos)
    {
        if (!pointDic.ContainsKey(pos))
            return null;

        return pointDic[pos];
    }
    
    public Point GetPointByPos(Vector3 pos)
    {
        Vector3Int pointPos = VTool.ToPointPos(pos);
        if (!pointDic.ContainsKey(pointPos))
            return null;

        return pointDic[pointPos];
    }
    
    public bool HadThisPoint(Vector3Int pos)
    {
        return pointDic.ContainsKey(pos);
    }
    
    public void AddTileDic(Point point)
    {
        pointDic.Add(point.Pos,point);
    }

    public void ChangeGameState(GameState gameState)
    {
        _gameState = gameState;
    }
}
