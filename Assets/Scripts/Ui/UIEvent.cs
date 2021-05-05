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
        
        private List<TowerType> chooseTypes;
        private Dictionary<TowerType, GameObject> upgradeUiDic;
        
        
        public void Init()
        {
            chooseTypes = GameManager.Instance.choosedTowerList;
            
            chooseTowerPanel = transform.Find("ChooseTower") as RectTransform;
            settingPanel = transform.Find("Setting") as RectTransform;
            upGradeTowerPanel = transform.Find("UpGradeTower") as RectTransform;
            playerMsgPanel = transform.Find("PlayerMsg") as RectTransform;

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
        
        
        public void ChickChoose(int index)
        {
            TowerType type = chooseTypes[index];
            upGradeTowerPanel.anchoredPosition = chooseTowerPanel.anchoredPosition;
            

            GameObject tower = Instantiate(AssestMgr.Instance.GetTowerBaseObj(type));
            BaseTower baseTower = tower.GetComponent<BaseTower>();

            tower.SetActive(true);
            tower.transform.position = UiManager.Instance.curPoint.CenterPos;
            tower.GetComponent<SpriteRenderer>().sortingOrder = UiManager.Instance.curPoint.Y;
            
            UiManager.Instance.curPoint.SetTower(baseTower);

        }

        public void ShowUpgradeBtn(BaseTower tower)
        {
            for(int i=0; i<upGradeTowerPanel.childCount ; i++)
                DestroyImmediate(upGradeTowerPanel.GetChild(i).gameObject);

                        
            for (int i = 0; i<tower.curTower.nextLevelTower.Length; i++)
            {
                
            }
        } 

        public void SetUpGredeUi(Tower tower)
        {
            
            for (int i = 0; i < upGradeTowerPanel.childCount; i++)
            {
                
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
        
        public List<GameObject> SetCirclePanel(RectTransform parent,GameObject orig,List<Sprite> spriteList, float startAngle = 58.12f)
        {
            List<GameObject> objList = new List<GameObject>();

            if (spriteList.Count == 1)
            {
                GameObject obj = Instantiate(orig);
                obj.GetComponent<Image>().sprite = spriteList[0];
                RectTransform rect = obj.transform as RectTransform;
                rect.SetParent(chooseTowerPanel, false);
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = Vector2.zero;
                objList.Add(obj);
                return objList;
            }
            
            float radius = 50;
            float deltaTheta = (2f * Mathf.PI) / spriteList.Count;
            float theta = startAngle; //当前角度
            
            for (int i = 0; i < spriteList.Count; i++)
            {
                Vector2 pos = parent.anchoredPosition +
                              new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
                theta = deltaTheta + theta;
                GameObject obj = Instantiate(orig);
                obj.GetComponent<Image>().sprite = spriteList[i];
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
