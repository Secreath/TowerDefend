using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : Singleton<UiManager>
{

    public Camera UiCamera
    {
        get { return canvas.worldCamera; }
    }
    
    private Canvas canvas;
    private RectTransform chooseTower;

    private Point prePoint;
    private Vector3 preVec3;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {

        chooseTower = transform.Find("ChooseTower") as RectTransform;
    }

    public void ChangeChooseUiPos()
    {
        
    }

    public void CheckUiBehavior(Point point, Vector3 towerPos)
    {
        if (prePoint == point && chooseTower.gameObject.activeSelf)
        {
            chooseTower.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("false");
            chooseTower.gameObject.SetActive(true);
            MoveTowerUi(towerPos);
        }

        prePoint = point;
    }
    
    public void MoveTowerUi(Vector3 towerPos)
    {
        Debug.Log("2");
        if (preVec3 == towerPos && chooseTower.gameObject.activeSelf)
        {
            chooseTower.gameObject.SetActive(false);
        }
        else
        {
            chooseTower.gameObject.SetActive(true);
        
        }

        preVec3 = towerPos;
        
        //使用场景相机将世界坐标转换为屏幕坐标
        Vector2 screenUiPos = UiCamera.WorldToScreenPoint(towerPos);
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, 
            screenUiPos,
            UiCamera, 
            out Vector2 retPos);
        //对应的锚点坐标
        chooseTower.anchoredPosition = retPos;
        Debug.Log(retPos);
     
    }

}
