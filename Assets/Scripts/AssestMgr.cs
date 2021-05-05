using System.Collections;
using System.Collections.Generic;
using tower;
using ui;
using UnityEngine;

public class AssestMgr : Singleton<AssestMgr>
{
    private Dictionary<TowerType, Tower> towerDic;
    private Dictionary<TowerType, GameObject> towerBaseObjDic;
    private Dictionary<TowerType, GameObject> chooseUiDic;
    private Dictionary<TowerType, List<List<GameObject>>> upgradeUiDic;
    private GameObject upgradeBtn;
    
    private int LoadCount;
    private int curLoad;

    private float curProgress
    {
        get { return (float) curLoad / LoadCount; }
    }
    
    private bool loadComplete;

    public bool LoadComplete => loadComplete;
    
    public void Init(List<TowerType> choosedTowerList)
    {
        int typeCount =(int)TowerType.End;
        curLoad = 0;
        LoadCount = typeCount * ((int)UiType.End + 2); 
        StartCoroutine(LoadAllTowerObj(choosedTowerList));
    }
    
    private IEnumerator LoadAllTowerObj(List<TowerType> choosedTowerList)
    {
        loadComplete = false;
        towerDic = new Dictionary<TowerType, Tower>();
        towerBaseObjDic = new Dictionary<TowerType, GameObject>();
        chooseUiDic = new Dictionary<TowerType, GameObject>();
        upgradeUiDic = new Dictionary<TowerType, List<List<GameObject>>>();
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
    
    private int SetTowerLevel(TowerMsg tower,int curLevel)
    {
        if (tower == null)
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
    
//    public GameObject GetUpgradeUiObj(TowerType type)
//    {
//        if (upgradeUiDic.ContainsKey(type))
//            return upgradeUiDic[type];
//        return null;
//    }
    
}
