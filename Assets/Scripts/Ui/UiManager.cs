using System;
using System.Collections;
using System.Collections.Generic;
using mapThing;
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
        
        internal Point curPoint;

        private UIEvent uiEvent;
        private Canvas canvas;
        private RectTransform chooseTowerPanel;
        private RectTransform settingPanel;
        private RectTransform upGradeTowerPanel;
        private Stack<GameObject> UiStack;
        private GameObject preUiObj;
        private UiState uiState;

        //height
        private List<Transform> childList;

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

        public void Init()
        {
            canvas = GetComponent<Canvas>();
            
            UiStack = new Stack<GameObject>();
            chooseTowerPanel = transform.Find("ChooseTower") as RectTransform;
            settingPanel = transform.Find("Setting") as RectTransform;
            upGradeTowerPanel = transform.Find("UpGradeTower") as RectTransform;
            hightLightRect = transform.Find("HightLight") as RectTransform;
            uiEvent = gameObject.AddComponent<UIEvent>();
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
                    GameManager.Instance.ChangeGameState(GameState.PlayGame);
                }
                return;
            }
            
            UiStack.Push(settingPanel.gameObject);
            GameManager.Instance.ChangeGameState(GameState.OpenUi);
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
            GameManager.Instance.ChangeGameState(GameState.PlayGame);
        }
        
        public void ShowTowerUi(Vector3 pos)
        {
            Vector3Int pointPos = VTool.ToPointPos(pos);
            if (!GameManager.Instance.HadThisPoint(pointPos))
                return;
            
            curPoint = GameManager.Instance.GetPointByPos(pointPos);
            
            if(curPoint.Tile.TileType ==TileType.road)
                return;
            
            if (!curPoint.HadTower)
                SetTowerUiPos(UiState.OpenChoosePanel, curPoint);
            else
                SetTowerUiPos(UiState.OpenUpGradePanel, curPoint);

        }

        public void SetTowerUiPos(UiState uiType,Point point)
        {
            //使用场景相机将世界坐标转换为屏幕坐标
            Vector2 screenUiPos = UiCamera.WorldToScreenPoint(point.CenterPos);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform, 
                screenUiPos,
                UiCamera, 
                out Vector2 retPos);
            OpenPanel(uiType,retPos);
        }

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
            if(uiState != UiState.OpenChoosePanel &&  uiState != UiState.OpenUpGradePanel)
                return;
            
            childList = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
                childList.Add(parent.GetChild(i));
            
            curChooseIndex = 0;
            hightLightRect.position = parent.GetChild(0).position;
        }

        public void PreOrNext(bool next)
        {
            if(GameManager.Instance.gameState != GameState.OpenUi)
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
            GameManager.Instance.ChangeGameState(GameState.OpenUi);
            
            switch (uiState)
            {
                case UiState.Close:
                    ShowTowerUi(pos);
                    break;
                case UiState.OpenChoosePanel:
                    uiEvent.ChickChoose(curChooseIndex);
                    EscAllUi();
                    break;
                case UiState.OpenUpGradePanel:
                    break;
                case UiState.OpenSettingPanel:
                    break;
            }
        }
        
    }
    

}
