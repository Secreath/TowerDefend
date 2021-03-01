using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TDRode : MonoBehaviour
{
    public Material lineMat;
    public List<PathLine> pathLines;
    
    
    private Transform start;                //起始路线
    private Transform inflection;        //拐点
    private Transform path;                    //路线
    private Transform finish;            //终点

    private List<Transform> inflections = new List<Transform>();
    private List<List<Transform>> starts = new List<List<Transform>>();
    private List<List<Transform>> paths = new List<List<Transform>>();
    private List<List<Transform>> finishs = new List<List<Transform>>();
    
    
    private List<LineRenderer> lineRenderer = new List<LineRenderer>();

    private int iLength = 0;
    private int sLength = 0;
    private int pLength = 0;
    private int fLength = 0;
    void Start()
    {
        Debug.Log(pathLines[0].pathStr + " " +pathLines[0].path.Count);
        start = transform.Find("Start");
        inflection = transform.Find("Inflection");
        path = transform.Find("Path");
        finish = transform.Find("Finish");
        InitLineRender();
        SetRoad();
    }

    private void SetRoad()
    {
        FindInflection();
        FindRode(starts,start);
        FindRode(paths,path);
        FindRode(finishs,finish);
        for (int i = 0; i < pathLines.Count; i++)
            AnalysisPathStr(pathLines[i]);
        RenderLine();
    }
    
    private void InitLineRender()
    {
        GameObject lineParent = new GameObject("Line");
        lineParent.transform.parent = transform;
        lineParent.transform.localPosition = Vector3.zero;

        
        for (int i = 0; i < pathLines.Count; i++)
        {
            GameObject line = new GameObject("line" + i.ToString());
            line.transform.parent = lineParent.transform;
            line.transform.localPosition = Vector3.zero;
            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.startColor = pathLines[i].color;
            lr.endColor = pathLines[i].color;
            lr.startWidth = 0.2f;
            lr.endWidth = 0.2f;
            lr.material = lineMat;
            
            lineRenderer.Add(lr);
            pathLines[i].lineRenderer = lr;
        }
    }
    private void FindInflection()
    {
        inflections.Clear();
        for (int i = 0; i < inflection.childCount; i++)
        {
            inflections.Add(inflection.GetChild(i));
        }
    }
    private void FindRode(List<List<Transform>> pathList,Transform parent)
    {
        pathList.Clear();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform path = parent.GetChild(i);
            pathList.Add(new List<Transform>());
            for (int j = 0; j < path.childCount; j++)
            {
                pathList[i].Add(path.GetChild(j));
            }
        }
    }
    
    private void AnalysisPathStr(PathLine pathLine)
    {
        string[] strList = pathLine.pathStr.Split(',');
        if(strList.Length < 1)
            return;
        
        pathLine.path.Clear();
        for (int i = 0; i < strList.Length; i++)
        {
            string temp = strList[i].Substring(1, strList[i].Length - 1);
            if (!Int32.TryParse(temp, out int index))
            {
                Debug.LogError("路线格式错误");
                return;
            }
            switch (strList[i][0])
            {
                case 's':
                case 'S':
                    if (index > starts.Count - 1)
                    {
                        Debug.LogError("当前序号长度超出starts长度");
                        return;
                    }
                    pathLine.path.AddRange(starts[index]);
                    break;
                case 'i':
                case 'I':
                    if (index > inflections.Count - 1)
                    {
                        Debug.LogError("当前序号长度超出inflect长度");
                        return;
                    }
                    pathLine.path.Add(inflections[index]);
                    break;
                case 'p':
                case 'P':
                    if (index > paths.Count - 1)
                    {
                        Debug.LogError("当前序号长度超出paths长度");
                        return;
                    }
                    pathLine.path.AddRange(paths[index]);
                    break;
                case 'f':
                case 'F':
                    if (index > finishs.Count - 1)
                    {
                        Debug.LogError("当前序号长度超出finishs长度");
                        return;
                    }
                    if(i != strList.Length-1)
                    {
                        Debug.LogError("当前路线finish节点不是最后一步");
                    }    
                    pathLine.path.AddRange(finishs[index]);
                    return;
            }
        }
        
    }
    
    private void RenderLine()
    {
        for (int i = 0; i < pathLines.Count; i++)
        {
            pathLines[i].lineRenderer.positionCount = pathLines[i].path.Count;
            for (int j = 0; j < pathLines[i].path.Count; j++)
            {
                Vector3 pos = pathLines[i].path[j].position;
                pathLines[i].lineRenderer.SetPosition(j,pos);
            }
        }
        
        
    }
    
    void Update()
    {
        SetRoad();
    }
}

[Serializable]
public class PathLine
{
    [HideInInspector] 
    public List<Transform> path;
    [HideInInspector]
    public LineRenderer lineRenderer;
    public string pathStr;
    public Color32 color;
    
    
    public PathLine()
    {
        pathStr = default;
        color = default;
        path = new List<Transform>();
        lineRenderer = new LineRenderer();
    }

}
