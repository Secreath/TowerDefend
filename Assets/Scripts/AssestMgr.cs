﻿using System;
using System.Collections;
using System.Collections.Generic;
using level;
using tower;
using ui;
using UnityEngine;

public class AssestMgr : Singleton<AssestMgr>
{
    private Dictionary<TowerType, Tower> towerDic;
    private Dictionary<TowerType, GameObject> towerBaseObjDic;
    private Dictionary<TowerType, GameObject> chooseUiDic;
    private Dictionary<int, LevelMsg> levelMsgDic;
    
    private int LoadCount;
    private int curLoad;

    private float curProgress
    {
        get { return (float) curLoad / LoadCount; }
    }
    
    private bool loadComplete;

    public bool LoadComplete => loadComplete;
    
    public void Init(int level)
    {
        towerDic = new Dictionary<TowerType, Tower>();
        towerBaseObjDic = new Dictionary<TowerType, GameObject>();
        chooseUiDic = new Dictionary<TowerType, GameObject>();
        levelMsgDic = new Dictionary<int, LevelMsg>();
        
        int typeCount =(int)TowerType.End;
        curLoad = 0;
        LoadCount = typeCount * ((int)UiType.End + 2);

        LoadLevelRes(level);
    }

    public void LoadLevelRes(int level)
    {
        List<TowerType> choosedTowerList = new List<TowerType>();
        string[] strs = LoadLevelMsg(level).chooseTowers.Split(',');
        for (int i = 0; i < strs.Length; i++)
        {
            if (int.TryParse(strs[i], out int type))
            {
                TowerType towerType = (TowerType) type;
                if(choosedTowerList.Contains(towerType) 
                   || towerType == TowerType.UpGrade || towerType == TowerType.End)
                    continue;
                choosedTowerList.Add(towerType);
            }
        }
        
        StartCoroutine(LoadAllTowerObj(choosedTowerList));
    }
    
    
    private IEnumerator LoadAllTowerObj(List<TowerType> choosedTowerList)
    {
        loadComplete = false;
        ResourceRequest rr;
        
        for (int i = 0; i < choosedTowerList.Count; i++)
        {
            TowerType towerType = choosedTowerList[i];
            Tower tower = Resources.Load<Tower>("ScriptableObject/Tower/" + towerType.ToString() + "Tower");
            
            Debug.Log(tower.ToString());
            towerDic.Add(towerType,tower);
            tower.maxLevel = SetTowerLevel(tower.towerMsg, 0);
            
            
            rr = Resources.LoadAsync<GameObject>("Tower/" + towerType.ToString() + "Tower");
            yield return rr;
            curLoad++;
            towerBaseObjDic.Add(towerType,rr.asset as GameObject);
            
            for (int j = 0; j != (int) UiType.End; j++)
            {
                UiType uitype = (UiType) j;
                string uiType = uitype.ToString();
                    
                rr = Resources.LoadAsync<GameObject>("UI/" + uiType + "/" + towerType.ToString() + uiType);
                yield return rr;
                switch (uitype)
                {
                    case UiType.ChooseBtn:
                        chooseUiDic.Add(towerType,rr.asset as GameObject);
                        break;
                    case UiType.UpGradeBtn:
//                        upgradeUiDic.Add(towerType,rr.asset as GameObject);
                        break;
                }
                curLoad++;
            }
        }

        loadComplete = true;
        
        GameManager.Instance.Init();
    }


//    private IEnumerator LoadAllUpgradeTowerObj(Tower tower, UiType uiType)
//    {
//        List<List<GameObject>> thisType = new List<List<GameObject>>();
//        for (int i = 0; i < tower.maxLevel; i++)
//        {
//            thisType.Add(new List<GameObject>());
//            for(tower.tower)
//        }
//    }

    public LevelMsg LoadLevelMsg(int level)
    {
        if (levelMsgDic.ContainsKey(level))
            return levelMsgDic[level];
        
        LevelMsg ls = Resources.Load<LevelMsg>("ScriptableObject/Level/LevelSpawn" + level.ToString());

        if(ls == null)
            throw new NullReferenceException($"本地未包含 LevelSpawn{level}");
        levelMsgDic.Add(level,ls);
        return ls;
    }
    private int SetTowerLevel(TowerMsg tower,int curLevel)
    {
        if (tower.nextLevelTower == null)
            return curLevel;
        tower.curLevel = curLevel;
        int maxLevel = 0;
        for (int i = 0; i < tower.nextLevelTower.Length; i++)
        {
            maxLevel =  SetTowerLevel(tower.nextLevelTower[i], curLevel + 1);
        }

        return curLevel > maxLevel? curLevel : maxLevel;
    }
    

    public Tower GetTower(TowerType type)
    {
        if (towerDic.ContainsKey(type))
            return towerDic[type];
        return null;
    }
    
    public GameObject GetTowerBaseObj(TowerType type)
    {
        if (towerBaseObjDic.ContainsKey(type))
            return towerBaseObjDic[type];
        return null;
    }
    
    public GameObject GetChooseUiObj(TowerType type)
    {
        if (chooseUiDic.ContainsKey(type))
            return chooseUiDic[type];
        return null;
    }

}
