using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Road : Singleton<Road>
{
    public Material lineMat;

    private List<LineRenderer> lineList;

    public List<List<Transform>> roadList;

    private Transform lineParent;

    private Transform roadParent;
    // Start is called before the first frame update
    void Start()
    {
        lineList = new List<LineRenderer>();
        
        CheckParent();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            AddLineRender(i);
        }
        
    }


    void CheckParent()
    {
        if (transform.Find("Road") == null)
        {
            roadParent = new GameObject("Road").transform;
            roadParent.transform.parent = transform;
            roadParent.transform.localPosition = Vector3.zero;
        }
        
        if (transform.Find("Line") == null)
        {
            lineParent = new GameObject("Line").transform;
            lineParent.transform.parent = transform;
            lineParent.transform.localPosition = Vector3.zero;
        }
        
        roadParent = transform.Find("Road");
        lineParent = transform.Find("Line");
    }
    void FindRoad()
    {
        CheckParent();
        
        roadList  = new List<List<Transform>>();
        lineList = new List<LineRenderer>();
        for (int i = 0; i < roadParent.childCount; i++)
        {
            Transform parent = roadParent.GetChild(i);
            AddLineRender(i);
            lineList.Add(lineParent.GetChild(i).GetComponent<LineRenderer>());
            List<Transform> pointList = new List<Transform>();
            FindPoint(i, parent, pointList);
            roadList.Add(pointList);
        }
        
        
    }

    private void AddLineRender(int index)
    {
        if(lineParent.childCount >= roadParent.childCount)
            return;
        
        GameObject line = new GameObject("line" + index.ToString());
        line.transform.parent = lineParent.transform;
        line.transform.localPosition = Vector3.zero;
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.startColor = ColorTool.RandomColor(1,0.5f,1);
        lr.endColor = lr.startColor;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.material = lineMat;
        
        lineList.Add(lr);
    }
    
    void FindPoint(int index,Transform parent,List<Transform> pointList)
    {
        lineList[index].positionCount = parent.childCount;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            pointList.Add(child);
            Vector3 pos = VTool.ToTilePos(child.position);
            lineList[index].SetPosition(i,pos);
        }
    }
    // Update is called once per frame
    void Update()
    {
        FindRoad();
    }
}
