using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Vector2 CenterPos
    {
        get { return transform.position; }
    }

    public bool HadBuild
    {
        get { return hadBuild; }
    }

    private bool hadBuild;
    public Point point { get; set; }

    public TileType type;
    
    
    
    
    private Vector2 size;
    private void Start()
    {
        size = GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    public void SetUp(Point gridPos,Vector3 worldPos,Transform parent)
    {
        this.point = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.tileDic.Add(gridPos,this);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower();
        }
    }


    private void PlaceTower()
    {
        if(type != TileType.Build)
            return;
        
        UiManager.Instance.CheckUiBehavior(point,CenterPos);
        
        if(hadBuild)
           return;

        hadBuild = true;
        GameObject tower = Instantiate(GameManager.Instance.TowerPrefab,CenterPos,Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = point.Y;

    }
}
