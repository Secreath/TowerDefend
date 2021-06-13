using System.Collections;
using System.Collections.Generic;
using tower;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui
{
    public class UIEvent : MonoBehaviour
    {

        private RectTransform chooseTowerPanel;
        private RectTransform settingPanel;
        private RectTransform upGradeTowerPanel;
        private RectTransform playerMsgPanel;
        private GameObject upgradeBtn;
        private List<TowerType> chooseTypes;
        private Dictionary<TowerType, GameObject> upgradeUiDic;
        private UpGradeTower curTower;
        
        public void Init()
        {
            chooseTypes = GameManager.Instance.choosedTowerList;
            
            chooseTowerPanel = transform.Find("ChooseTower") as RectTransform;
            settingPanel = transform.Find("Setting") as RectTransform;
            upGradeTowerPanel = transform.Find("UpGradeTower") as RectTransform;
            playerMsgPanel = transform.Find("PlayerMsg") as RectTransform;
            upgradeBtn = transform.Find("UpgradeBtn").gameObject;
            InitChooseTowerPanel();
//            InitUpGradePanel();
        }

        public void LoadAllNextLevelTower()
        {
            
        }
        
        public void InitChooseTowerPanel(float startAngle = 58.12f)
        {
            
            int count = chooseTypes.Count;
            float radius = 50;
            float deltaTheta = (2f * Mathf.PI) / count;
            float theta = startAngle; //当前角度
            for (int i = 0; i < chooseTypes.Count; i++)
            {
                TowerType type = chooseTypes[i];
                
                Vector2 pos = chooseTowerPanel.anchoredPosition +
                              new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
                GameObject obj = Instantiate(AssestMgr.Instance.GetChooseUiObj(type));
                theta = deltaTheta + theta;

                RectTransform rect = obj.transform as RectTransform;
                rect.SetParent(chooseTowerPanel, false);
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = pos;
//                Button btn = obj.GetComponent<Button>();
//                btn.onClick.AddListener(() =>
//                {
////                    UiManager.Instance.UpdateUiStackHead(upGradeTowerPanel.gameObject);
//                    
//                });
            }
        }

//        private void InitUpGradePanel()
//        {
//            upgradeUiDic = new Dictionary<TowerType, GameObject>();
//            
//            for (int i = 0; i < chooseTypes.Count; i++)
//            {
//                TowerType type = chooseTypes[i];
//                
//                
//                GameObject upGradeBtn = Instantiate(AssestMgr.Instance.GetUpgradeUiObj(type));
//                
//                upGradeBtn.transform.SetParent(upGradeTowerPanel, false);
//                upGradeBtn.GetComponent<Button>().onClick.AddListener(()=>
//                {
//                    UiManager.Instance.curPoint.BaseTower.UpGrade();
//                });
//                upGradeBtn.SetActive(false);
//                upgradeUiDic.Add(type,upGradeBtn);
//                
//            }
//        }
        
        
        public void ClickChoose(int index)
        {
            TowerType type = chooseTypes[index];
            
            Debug.Log("Choose " + type + " " + GameManager.TypeDic[type] + " " + UiManager.curPoint.tileType);
            if (GameManager.TypeDic[type] != UiManager.curPoint.tileType)
            {
                return;
            }
            upGradeTowerPanel.anchoredPosition = chooseTowerPanel.anchoredPosition;

            GameObject tower = Instantiate(AssestMgr.Instance.GetTowerBaseObj(type));
            BaseTower baseTower = tower.GetComponent<BaseTower>();
            tower.SetActive(true);
            tower.transform.position = UiManager.curPoint.CenterPos;
            tower.GetComponent<SpriteRenderer>().sortingOrder = UiManager.curPoint.Y;
            GameManager.Instance.RegisterTower(tower, type);
            UiManager.curPoint.SetTower(baseTower);
        }

        public bool CheckUpgrade(int index)
        {
            return curTower.BuildTower.CheckCanUpGrade(index);
        }
        public void ClickUpGrade()
        {
           curTower.UpGrade();
        }
        

        public void ShowUpgradeBtn(UpGradeTower upGradeTower)
        {
            for (int i = upGradeTowerPanel.childCount -1 ; i >=0; i--)
            {
                DestroyImmediate(upGradeTowerPanel.GetChild(i).gameObject);
            }
            curTower = upGradeTower;
            BaseTower tower = upGradeTower.BuildTower;
            List<GameObject> upgradeList = SetCirclePanel(upGradeTowerPanel, upgradeBtn, tower.CurTower.nextLevelTower.Length);
            
            
            for (int i = 0; i<tower.CurTower.nextLevelTower.Length; i++)
            {
                upgradeList[i].transform.Find("Image").GetComponent<Image>().sprite =
                    tower.CurTower.nextLevelTower[i].towerSprite;
                
                upgradeList[i].transform.Find("CoinText").GetComponent<Text>().text =
                    tower.CurTower.nextLevelTower[i].buildPrice.ToString();
                
                upgradeList[i].transform.Find("TimeText").GetComponent<Text>().text =
                    tower.CurTower.nextLevelTower[i].buildTime.ToString();
            }
        } 
        
        public void SetCirclePos(RectTransform parent,GameObject obj,int curent,int count, float startAngle = 58.12f)
        {
            float radius = 50;
            float deltaTheta = (2f * Mathf.PI) / count;
            float theta = startAngle; //当前角度
            
            theta = curent * deltaTheta + theta;
            Vector2 pos = parent.anchoredPosition +
                        new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
            RectTransform rect = obj.transform as RectTransform;
            rect.anchoredPosition = pos;
        }
        
        public List<GameObject> SetCirclePanel(RectTransform parent,GameObject orig,int count)
        {
            List<GameObject> objList = new List<GameObject>();

            float startAngle = count > 2 ? 58.12f : 0;
            
            if (count == 1)
            {
                GameObject obj = Instantiate(orig);
                RectTransform rect = obj.transform as RectTransform;
                rect.SetParent(parent, false);
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = Vector2.zero;
                objList.Add(obj);
                return objList;
            }
            
            float radius = 50;
            float deltaTheta = (2f * Mathf.PI) / count;
            float theta = startAngle; //当前角度
            
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = parent.position +
                              new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta),0f);
                theta = deltaTheta + theta;
                GameObject obj = Instantiate(orig);
                RectTransform rect = obj.transform as RectTransform;
                rect.SetParent(parent, false);
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = pos;
                objList.Add(obj);
            }
            return objList;
        }
    }

}
