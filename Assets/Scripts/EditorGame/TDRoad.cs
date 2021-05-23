using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class TDRoad : Singleton<TDRoad>
{
    public Material lineMat;
    public List<PathLine> pathLines = new List<PathLine>();
    
    
    private Transform start;                //起始路线
    private Transform inflection;        //拐点
    private Transform path;                    //路线
    private Transform finish;            //终点

    public Dictionary<int,List<Point>> PointsDic = new Dictionary<int, List<Point>>();
    
    
    private List<Transform> inflections = new List<Transform>();
    private List<List<Transform>> starts = new List<List<Transform>>();
    private List<List<Transform>> paths = new List<List<Transform>>();
    private List<List<Transform>> finishs = new List<List<Transform>>();
    
    
    private List<LineRenderer> lineRenderer = new List<LineRenderer>();

    private int iLength = 0;
    private int sLength = 0;
    private int pLength = 0;
    private int fLength = 0;

    [HideInInspector]
    public List<Transform> startTrans = new List<Transform>();
    [HideInInspector]
    public List<Transform> finishTrans = new List<Transform>();
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
        FindRoad(starts,start);
        FindRoad(paths,path);
        FindRoad(finishs,finish);
        for (int i = 0; i < pathLines.Count; i++)
            AnalysisPathStr(pathLines[i]);
        RenderLine();
    }
    
    private void InitLineRender()
    {
        if(transform.Find("Line") != null)
            return;
        
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
    private void FindRoad(List<List<Transform>> pathList,Transform parent)
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
            if(path.childCount > 0 && parent == start && !startTrans.Contains(path.GetChild(0)))
                startTrans.Add(path.GetChild(0));
            if(path.childCount > 0 && parent == finish && !finishTrans.Contains(path.GetChild(0)))
                finishTrans.Add(path.GetChild(0));
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
                Vector3 pos = VTool.ToTilePos(pathLines[i].path[j].position);
                pathLines[i].lineRenderer.SetPosition(j,pos);
            }
        }
        
        
    }
    
    void Update()
    {
        SetRoad();
    }

    public static List<Transform> GetPathList(int roadID)
    {
        
        if (Instance.pathLines.Count > roadID)
            return Instance.pathLines[roadID].path;
        
        return new List<Transform>();
    }
    
    public static List<Point> GetPointLine(int roadID)
    {
        if (Instance.pathLines.Count <= roadID)
            return new List<Point>();

        if (!Instance.PointsDic.ContainsKey(roadID))
        {
            List<Point> pointList = new List<Point>();

            for (int i = 0; i < Instance.pathLines[roadID].path.Count; i++)
            {
                Vector3 pos = Instance.pathLines[roadID].path[i].position;
                pointList.Add(GameManager.GetPointByPos(pos));
            }
            Instance.PointsDic.Add(roadID,pointList);
        }
        return Instance.PointsDic[roadID];
    }
    
}

[Serializable]
public class PathLine
{
    public int pathID;
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
