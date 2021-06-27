using System;
using System.Collections;
using System.Collections.Generic;
using mapThing;
using plugin.ugui.view;
using tower;
using UnityEngine;
using UnityEngine.UI;

namespace ui
{
    public class UiManager : Singleton<UiManager>
    {

        public Camera UiCamera
        {
            get { return canvas.worldCamera; }
        }
        
        internal static Point curPoint;

        private UIEvent uiEvent;
        private Canvas canvas;
        private RectTransform chooseTowerPanel;
        private RectTransform settingPanel;
        private RectTransform upGradeTowerPanel;
        private Stack<GameObject> UiStack;
        private GameObject preUiObj;
        private UiState uiState;

        //PlayerPackage
        private RectTransform playerPackageRect;
        private Dictionary<ResType,Text> resTextDic;
        
        //height
        private List<Transform> childList;
        //timeCount
        private GameObject timer;
        private Transform timerList;
        private Transform timeCountPanel;
        //timeCount
        private List<GameObject> timeCountList;
        //EnemyUi
        public static EnemyUiManager EnemyUi;
        private int _curChooseIndex;
        private int curChooseIndex
        {
            get { return _curChooseIndex; }
            set
            {
                if (childList == null)
                {
                    _curChooseIndex = 0;
                    return;
                }

                if (value < 0)
                    _curChooseIndex = childList.Count - 1;
                else if (value >= childList.Count)
                    _curChooseIndex = 0;
                else
                    _curChooseIndex = value;
            }
        }
        private RectTransform hightLightRect;

        public void Start()
        {
            canvas = GetComponent<Canvas>();
            
            UiStack = new Stack<GameObject>();
            timeCountList = new List<GameObject>();
            chooseTowerPanel = transform.Find("ChooseTower") as RectTransform;
            settingPanel = transform.Find("Setting") as RectTransform;
            upGradeTowerPanel = transform.Find("UpGradeTower") as RectTransform;
            hightLightRect = transform.Find("HightLight") as RectTransform;
            
            playerPackageRect = transform.Find("PlayerPackage") as RectTransform;
            
            resTextDic = new Dictionary<ResType, Text>();
            for (int i = 0; i != (int)ResType.End; i++)
            {
                ResType type = (ResType) i;
                Text text = playerPackageRect.Find(type.ToString()).Find("Text").GetComponent<Text>();
                if(text!=null)
                    resTextDic.Add(type,text);
            }
            
            timerList = transform.Find("TimerList");
            timeCountPanel = transform.Find("TimeCountPanel");
            
            timer = timerList.Find("TimeCount").gameObject;
            uiEvent = gameObject.AddComponent<UIEvent>();
            EnemyUi = transform.Find("EnemyUi").GetComponent<EnemyUiManager>();
            
            
            EnemyUi.SetUiCamera(UiCamera);
        }

        public void Init()
        {
            uiEvent.Init();
        }
        
        public void EscPreUi()
        {
            if (UiStack.Count > 0)
            {
                preUiObj = UiStack.Pop(); 
                preUiObj.SetActive(false);
                if (UiStack.Count < 1)
                {
                    hightLightRect.gameObject.SetActive(false);
                    uiState = UiState.Close;
                    GameManager.ChangeGameState(GameState.PlayGame);
                }
                return;
            }
            
            UiStack.Push(settingPanel.gameObject);
            GameManager.ChangeGameState(GameState.OpenUi);
            settingPanel.gameObject.SetActive(true);
        }

        public void EscAllUi()
        {
            while (UiStack.Count > 0)
            {
                UiStack.Pop().gameObject.SetActive(false);
            }
            hightLightRect.gameObject.SetActive(false);
            uiState = UiState.Close;
            GameManager.ChangeGameState(GameState.PlayGame);
        }
        
        public void ShowTowerUi(Vector3 pos)
        {
            Vector3Int pointPos = VTool.ToPointPos(pos);
            if (!GameManager.Instance.HadThisPoint(pointPos))
                return;
            
            curPoint = GameManager.GetPointByPos(pointPos);
            
            if(curPoint.Tile.TileType ==TileType.road)
                return;
            
            if (!curPoint.HadTower)
                SetTowerUiPos(UiState.OpenChoosePanel, curPoint);
        }

        public void ShowUpgradeUi(UpGradeTower baseTower,Point point)
        {
            curPoint = point;
            uiEvent.ShowUpgradeBtn(baseTower);
            GameManager.ChangeGameState(GameState.OpenUi);
            SetTowerUiPos(UiState.OpenUpGradePanel, point);
        }

        public void SetTowerUiPos(UiState uiType,Point point)
        {
            OpenPanel(uiType,UiTool.GetTowerUiPos(transform,UiCamera,point));
        }

//        public Vector2 GetTowerUiPos(Point point)
//        {
//            //使用场景相机将世界坐标转换为屏幕坐标
//            Vector2 screenUiPos = UiCamera.WorldToScreenPoint(point.CenterPos);
//            
//            RectTransformUtility.ScreenPointToLocalPointInRectangle(
//                transform as RectTransform, 
//                screenUiPos,
//                UiCamera, 
//                out Vector2 retPos);
//            return retPos;
//        }
        
//        public void UpdateUiStackHead(GameObject panel)
//        {
//            UiStack.Pop().SetActive(false);
//            UiStack.Push(panel);
//            panel.SetActive(true);
//        }

        public void OpenPanel(UiState uiState,Vector2 rectPos)
        {
            GameObject panel = default;
            
            switch (uiState)
            {
                case UiState.OpenChoosePanel:
                    panel = chooseTowerPanel.gameObject;
                    chooseTowerPanel.anchoredPosition = rectPos;
                    break;
                case UiState.OpenUpGradePanel:
                    panel = upGradeTowerPanel.gameObject;
                    upGradeTowerPanel.anchoredPosition = rectPos;
                    break;
                case UiState.OpenSettingPanel:
                    panel = settingPanel.gameObject;
                    break;
            }
            
            UiStack.Push(panel);
            panel.gameObject.SetActive(true);
            
            RefreshChildList(panel.transform, uiState);
            this.uiState = uiState;
        }

        private void RefreshChildList(Transform parent,UiState uiState)
        {
            if(parent.childCount < 1 || (uiState != UiState.OpenChoosePanel &&  uiState != UiState.OpenUpGradePanel))
                return;
            childList = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
                childList.Add(parent.GetChild(i));
            
            curChooseIndex = 0;
            hightLightRect.position = parent.GetChild(0).position;
        }

        public void PreOrNext(bool next)
        {
            if(GameManager.gameState != GameState.OpenUi)
                return;
            
            hightLightRect.gameObject.SetActive(true);
            curChooseIndex = next ? curChooseIndex + 1:curChooseIndex - 1;
            if (childList.Count > curChooseIndex)
            {
                hightLightRect.position = childList[curChooseIndex].position;
//                hightLightRect.SetParent(childList[curChooseIndex]);
//                hightLightRect.localPosition = Vector3.zero;
            }
        }

        public void JKeyPress(Vector3 pos)
        {
            GameManager.ChangeGameState(GameState.OpenUi);
            
            Debug.Log("pressJ");
            switch (uiState)
            {
                case UiState.Close:
                    ShowTowerUi(pos);
                    break;
                case UiState.OpenChoosePanel:
                    uiEvent.ClickChoose(curChooseIndex);
                    EscAllUi();
                    break;
                case UiState.OpenUpGradePanel:
                    if (!uiEvent.CheckUpgrade(curChooseIndex))
                    {
                        Debug.Log(uiEvent.CheckUpgrade(curChooseIndex));
                        return;
                    }
                    uiEvent.ClickUpGrade();
                    EscAllUi();
                    break;
                case UiState.OpenSettingPanel:
                    break;
            }
        }

        public void CreateTimer(ref TimeCountDown timeCountDown,Point point)
        {
            
            GameObject newTimer = Instantiate(timer, timerList);
            newTimer.SetActive(false);
            (newTimer.transform as RectTransform).anchoredPosition = UiTool.GetTowerUiPos(transform,UiCamera,point);
            newTimer.name = "TimerCount" + timeCountList.Count;
            timeCountList.Add(newTimer);
            timeCountDown = newTimer.GetComponent<TimeCountDown>();
        }

        public void ShowTimeCountPanel(float time,string str)
        {
            timeCountPanel.gameObject.SetActive(true);
            TimeCountDown timeCountDown = timeCountPanel.Find("TimeCount").GetComponent<TimeCountDown>();
            timeCountDown.SetRefreshTime(time,ShowOver);
            timeCountPanel.Find("Text").GetComponent<Text>().text = str;
            void ShowOver()
            {
                timeCountPanel.gameObject.SetActive(false);
            }
        }
        
        public void ChangePackageUi(ResType res,int num)
        {
            resTextDic[res].text = num.ToString();
        }
    }
    

}
