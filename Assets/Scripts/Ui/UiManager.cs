using System;
using System.Collections;
using System.Collections.Generic;
using mapThing;
using Tower;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiManager : Singleton<UiManager>
    {

        public Camera UiCamera
        {
            get { return canvas.worldCamera; }
        }

        public List<TowerType> chooseTypes;
        
        
        internal Point curPoint;
        
        private Canvas canvas;
        private RectTransform chooseTowerPanel;
        private RectTransform settingPanel;
        private RectTransform upGradeTowerPanel;
        private Stack<GameObject> UiStack;

        private GameObject preUiObj;
        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            UiStack = new Stack<GameObject>();
            chooseTowerPanel = transform.Find("ChooseTower") as RectTransform;
            settingPanel = transform.Find("Setting") as RectTransform;
            upGradeTowerPanel = transform.Find("UpGradeTower") as RectTransform;
            
            UIEvent.Instance.Init();
        }

        public void EscPreUi()
        {
            if (UiStack.Count > 0)
            {
                preUiObj = UiStack.Pop(); 
                preUiObj.SetActive(false);
                return;
            }
            
            UiStack.Push(settingPanel.gameObject);
            settingPanel.gameObject.SetActive(true);
        }

        public void EscAllUi()
        {
            while (UiStack.Count > 0)
            {
                UiStack.Pop().gameObject.SetActive(false);
            }
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
                SetTowerUiPos(chooseTowerPanel, curPoint);
            else
                SetTowerUiPos(upGradeTowerPanel, curPoint);

        }

        public void ShowUpGradeTowerUi(Point point)
        {
            SetTowerUiPos(upGradeTowerPanel, point);
        }

        public void SetTowerUiPos(RectTransform uiRect,Point point)
        {
            UiStack.Push(uiRect.gameObject);
            
            uiRect.gameObject.SetActive(true);
            
            //使用场景相机将世界坐标转换为屏幕坐标
            Vector2 screenUiPos = UiCamera.WorldToScreenPoint(point.CenterPos);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform, 
                screenUiPos,
                UiCamera, 
                out Vector2 retPos);
            //对应的锚点坐标
            chooseTowerPanel.anchoredPosition = retPos;
        }
       
    }
    

}
