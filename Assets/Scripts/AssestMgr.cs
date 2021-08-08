using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using level;
using Player;
using SaveRes;
using tower;
using ui;
using UnityEngine;
using UnityEngine.UI;

public class AssestMgr : Singleton<AssestMgr>
{
    private Dictionary<TowerType, Tower> _towerDic;
    private Dictionary<TowerType, GameObject> _towerBaseObjDic;
    private Dictionary<TowerType, GameObject> _chooseUiDic;
    
    private Dictionary<int, LevelMsg> _levelMsgDic;
    private Dictionary<EnemyType, GameObject> _enemyDic;
    private Dictionary<ResType, BuildRes> _buildResDic;

    private MonsterModel _monsterModel;
    
    
    private int _loadCount;
    private int _curLoad;
    private float curProgress
    {
        get { return (float) _curLoad / _loadCount; }
    }
    
    private bool _loadComplete;

    public bool LoadComplete => _loadComplete;

    private LevelMsg _curLevelMsg;
    public LevelMsg CurLevelMsg => _curLevelMsg;
    
    public void Init(int level)
    {
        _towerDic = new Dictionary<TowerType, Tower>();
        _towerBaseObjDic = new Dictionary<TowerType, GameObject>();
        _chooseUiDic = new Dictionary<TowerType, GameObject>();
        _levelMsgDic = new Dictionary<int, LevelMsg>();
        _enemyDic = new Dictionary<EnemyType, GameObject>();
        _buildResDic = new Dictionary<ResType, BuildRes>();
        
        int typeCount =(int)TowerType.End;
        _curLoad = 0;
        _loadCount = typeCount * ((int)UiType.End + 2);
        
        
        LoadLevelRes(level);
        LoadMonsterJson();
    }

    public void LoadLevelRes(int level)
    {
        List<TowerType> choosedTowerList = new List<TowerType>();
        _curLevelMsg = LoadLevelMsg(level);
        string[] strs = _curLevelMsg.chooseTowers.Split(',');
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

        GameManager.Instance.choosedTowerList = choosedTowerList;
        StartCoroutine(LoadAllTowerObj(choosedTowerList));
    }

    public LevelPathData LoadLevelPathData(int level)
    {
        TextAsset text = Resources.Load<TextAsset>($"Path/path{level}");
        if(text == null)
            throw new NullReferenceException($"本地未包含 path{level}");
        LevelPathData pathData = JsonUtility.FromJson<LevelPathData>(text.text);
        return pathData;
    }
    
    public void LoadMonsterJson()
    {
        _monsterModel = new MonsterModel();
        
        TextAsset text = Resources.Load<TextAsset>($"Json/Monster");
        if(text == null)
            throw new NullReferenceException($"本地未包含 Json Monster");
        _monsterModel.InitMonster(text.text);
    }

    public static Monster GetMonsterFormDic(int id)
    {
        if (!Instance._monsterModel.monsterDic.ContainsKey(id))
            return null;
        return Instance._monsterModel.monsterDic[id];

    }
    private IEnumerator LoadAllTowerObj(List<TowerType> choosedTowerList)
    {
        _loadComplete = false;
        ResourceRequest rr;
        
        for (int i = 0; i < choosedTowerList.Count; i++)
        {
            TowerType towerType = choosedTowerList[i];
            Tower tower = Resources.Load<Tower>($"ScriptableObject/Tower/{towerType.ToString()}Tower");

            _towerDic.Add(towerType,tower);
            tower.maxLevel = SetTowerLevel(tower.towerMsg, 0);
            
            
            rr = Resources.LoadAsync<GameObject>($"Tower/{towerType.ToString()}Tower");
            yield return rr;
            _curLoad++;
            
            _towerBaseObjDic.Add(towerType,rr.asset as GameObject);
            
            for (int j = 0; j != (int) UiType.End; j++)
            {
                UiType uitype = (UiType) j;
                string uiType = uitype.ToString();

                rr = Resources.LoadAsync<GameObject>($"UI/{uiType}/{towerType.ToString()}{uiType}");
                yield return rr;
                switch (uitype)
                {
                    case UiType.ChooseBtn:
                        _chooseUiDic.Add(towerType,rr.asset as GameObject);
                        break;
                    case UiType.UpGradeBtn:
//                        upgradeUiDic.Add(towerType,rr.asset as GameObject);
                        break;
                }
                _curLoad++;
            }
        }

        //LoadBuildRes
        for (int i = 0; i != (int) ResType.End; i++)
        {
            ResType resType = (ResType) i;
            string resTypeStr  = resType.ToString();
            rr = Resources.LoadAsync<BuildRes>($"ScriptableObject/BuildResources/{resTypeStr}");
            yield return rr;
            if (!_buildResDic.ContainsKey(resType) && rr.asset != null)
            {
                _buildResDic.Add(resType,rr.asset as BuildRes);
            }
        }
        
        _loadComplete = true;
        
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
        if (_levelMsgDic.ContainsKey(level))
            return _levelMsgDic[level];
        
        LevelMsg ls = Resources.Load<LevelMsg>("ScriptableObject/Level/LevelMsg" + level.ToString());

        if(ls == null)
            throw new NullReferenceException($"本地未包含 LevelMsg{level}");
        _levelMsgDic.Add(level,ls);
        return ls;
    }

    public BuildRes LoadBuildRes(ResType type)
    {
        if(_buildResDic[type] == null)
            throw new NullReferenceException($"本地未包含 BuildRes{type}");
        return _buildResDic[type];
    }
    
    
    public GameObject LoadEnemy(EnemyType type)
    {
        if (_enemyDic.ContainsKey(type))
            return _enemyDic[type];
        GameObject go = Resources.Load<GameObject>($"Enemy/{type.ToString()}");

        if(go == null)
            throw new NullReferenceException($"本地未包含 {type}类型敌人");
        _enemyDic.Add(type,go);
        return go;
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
        if (_towerDic.ContainsKey(type))
            return _towerDic[type];
        return null;
    }
    
    public GameObject GetTowerBaseObj(TowerType type)
    {
        if (_towerBaseObjDic.ContainsKey(type))
        {
            return _towerBaseObjDic[type];
        }
        return null;
    }
    
    public GameObject GetChooseUiObj(TowerType type)
    {
        if (_chooseUiDic.ContainsKey(type))
            return _chooseUiDic[type];
        return null;
    }

}
