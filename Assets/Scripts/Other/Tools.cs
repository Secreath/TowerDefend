using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindObjectFromTag
{
    string tagName;
    float distance;        //范围之内
    Vector2 selfPos;   //GetPlayerPos().position;
    List<GameObject> findObjects;
    int enemtNums;

    public FindObjectFromTag(string tagName, Vector2 selfPos, float distance)
    {
        this.tagName = tagName;
        this.selfPos = selfPos;
        this.distance = distance;
    }


    public Transform ClosestTransform()
    {
        GameObject closestTarget;

        List<GameObject> temp = new List<GameObject>();                                         //暂存移除的obj
        findObjects = GameObject.FindGameObjectsWithTag(tagName).ToList();

        if (findObjects.Count == 0)
            return null;
        closestTarget = findObjects[0];

        float minDistanceSqr = ((Vector2)findObjects[0].transform.position - selfPos).sqrMagnitude;         //距离的平方        

        for (int i = 0; i < findObjects.Count; i++)
        {
            if (((Vector2)findObjects[i].transform.position - selfPos).sqrMagnitude > distance * distance)              //如果距离大于最大距离           
            {
                temp.Add(findObjects[i]);                           //添加进移除
                //continue;
            }
            else if (minDistanceSqr > ((Vector2)findObjects[i].transform.position - selfPos).sqrMagnitude)
            {
                minDistanceSqr = ((Vector2)findObjects[i].transform.position - selfPos).sqrMagnitude;
                closestTarget = findObjects[i];
            }
        }
        foreach (GameObject tempObj in temp)
        {
            findObjects.Remove(tempObj);
        }

        if (minDistanceSqr > distance * distance)                     //如果最小距离都大于攻击距离
        {
            return null;
        }

        enemtNums = findObjects.Count;
        return closestTarget.transform;
    }

    public List<GameObject> FindEnemyList(int listCount)
    {
        if (ClosestTransform() == null)
        {
            return null;
        }
        else if (listCount == 999)                                                                     //如果输入为999则对附近所有敌人进行攻击
        {
            listCount = enemtNums;
        }

        for (int i = 1; i < findObjects.Count; i++)                                     //根据距离对敌人进行排序
        {
            for (int j = i; j > 0; j--)
            {
                Vector2 point1 = findObjects[j - 1].transform.position;
                Vector2 point2 = findObjects[j].transform.position;
                //比较两个点的距离
                if ((point1 - selfPos).sqrMagnitude > (point2 - selfPos).sqrMagnitude)
                {
                    GameObject temp = findObjects[j - 1];
                    findObjects[j - 1] = findObjects[j];
                    findObjects[j] = temp;

                }
            }
        }

        for (int i = listCount; i < findObjects.Count; i++)
        {
            findObjects.Remove(findObjects[listCount - 1]);
        }
        if (listCount > enemtNums)                 //如果敌人数量小于标记数量       少的标记全部添加到最近的目标
        {
            int addnum = listCount - enemtNums;
            while (addnum > 0)
            {
                findObjects.Add(findObjects[0]);
                addnum--;
            }
        }
        return findObjects;
    }

    private static void swap<T>(ref T x, ref T y)
    {
        T temp = x;
        x = y;
        y = temp;
    }
}

public class Vec3ToVer3Int
{
    public Vector3Int transition(Vector3 vec3)
    {
        return new Vector3Int((int)vec3.x, (int)vec3.y, 0);
    }
}


public class GetMousePos
{
    public static Vector2 GetMousePosition()                                               //通过mainCamera获取
    {
        return  GetMousePositionWithZ();                                          //通过mainCamera获取
    }
    public static Vector3 GetMousePositionWithZ()                                               //通过mainCamera获取
    {
        return GetMousePositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMousePositionWithZ(Camera worldCamera)                 //通过指定Camera获取
    {
        return GetMousePositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMousePositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
public class GetAngle                                       //仅获取2d xy的夹角
{
    public static float  Angle(Vector3 target, Vector3 bullet)
    {
        Vector3 positionOne = target;
        Vector3 aimDirection =( target - bullet).normalized;

        //Vector2 relative = PointOne.transform.InverseTransformPoint(PointTwo).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;


        return angle;
    }
    
    public static void RotaWithZ(Vector3 target, Vector3 bullet,GameObject Obj)
    {
        Vector3 positionOne = target;
        Vector3 aimDirection =( target - bullet).normalized;

        //Vector2 relative = PointOne.transform.InverseTransformPoint(PointTwo).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        Obj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
}

public class Lookdir 
{
    public enum lookdir { down, right, up, left };
    public static lookdir GetLookDir(float angle)
    {

        float rota = angle - 90;
        if (-135 > rota && rota < 135)                 //下
        {
            return lookdir.down;
        }
        else if (-135 < rota && rota < -45)     //右
        {
            return lookdir.right;
        }
        else if (-45 < rota && rota < 45)    //上
        {
            return lookdir.up;
        }
        else                                                                    //左
        {
            return lookdir.left;
        }
    }
    
}

public class TimeToText
{
    //仅限分秒
    public static string time(int second)
    {
        int minute = 0;
        string minStr = "";
        string secStr = "";

        if (second > 60)
        {
            minute = second / 60;                   //119 /60 =1  119%60 =59 
            second %= 60;
        }
        if (minute < 10)
            minStr += "0";
        if (second < 10)
            secStr += "0";

        minStr += minute.ToString();
        secStr += second.ToString();

        return minStr + ":" + secStr;
    }
}

public class FindFiles 
{
    public static void FindObjPath(ref List<string> dirs, string targetPath = "")
    {
        DirectoryInfo root = new DirectoryInfo("Assets/Resources/" + targetPath);
        for (int i = 0; i < root.GetFiles().Length; i++)
        {
            if (Path.GetExtension(root.GetFiles()[i].Name) == ".prefab")
            {
                dirs.Add(targetPath +"/"+ Path.GetFileNameWithoutExtension(root.GetFiles()[i].Name));
                //Debug.Log(targetPath + Path.GetFileNameWithoutExtension(root.GetFiles()[i].Name));
            }
        }
    }
    #region GetDirs
    // 在菜单来创建 选项 ， 点击该选项执行搜索代码
    //[MenuItem("Tools/遍历项目所有文件夹")]
    //static void CheckSceneSetting()
    //{
    //    List<string> dirs = new List<string>();
    //    GetDirs(Application.dataPath, ref dirs);
    //}
    //参数1 为要查找的总路径， 参数2 保存路径
    /*
    public static void GetDirs(string dirPath, ref List<string> dirs, string targetPath = "")
    {

        foreach (string path in Directory.GetFiles(dirPath))
        {
            //    //获取所有文件夹中包含后缀为 .prefab 文件的路径
            //    //如果后缀为获取后缀为prefab的文件路径
            //Path.GetFileNameWithoutExtension(path)获取不带后缀的文件名
            if (path.Contains(targetPath) && Path.GetExtension(path) == ".prefab")
            {
                //添加相对路径 从Assests开始添加  
                //targetPath
                int startIndex = path.IndexOf(targetPath);
                int endIndex = path.IndexOf(".");
                dirs.Add(path.Substring(startIndex, endIndex - startIndex));
                Debug.Log(Path.GetFileNameWithoutExtension(path));
                Debug.Log(path.Substring(startIndex, endIndex-startIndex));

            }       

        }
        //遍历所有文件夹
        if (Directory.GetDirectories(dirPath).Length > 0)  
        {
            foreach (string path in Directory.GetDirectories(dirPath))
            {
                GetDirs(path, ref dirs, targetPath);
            }
        }
    }
    */
    #endregion
}

public class VTool
{
    public static Vector2 ToV2(Vector3 vector3)
    {
        return new Vector2(vector3.x,vector3.y);
    }
    public static Vector2Int ToV2Int(Vector3 vector3)
    {
        return new Vector2Int((int)vector3.x,(int)vector3.y);
    }
    
    public static Vector3Int ToV3Int(Vector3 vector3)
    {
        return new Vector3Int((int)vector3.x,(int)vector3.y,(int)vector3.z);
    }

    public static Vector3Int ToPointPos(Vector3 vector3)
    {
        int x = (int) vector3.x;
        int y = (int) vector3.y;

        if (vector3.x < 0)
            x -= 1;
        if (vector3.y < 0)
            y -= 1;
        return new Vector3Int(x,y,0);
    }
    
    public static Vector3Int ToPointPos(Vector2 vector2)
    {
        return ToPointPos(new Vector3(vector2.x,vector2.y,0));
    }
    
    
    public static Vector3 ToTilePos(Vector3 vector3)
    {
        return ToPointPos(vector3) + new Vector3(0.5f, 0.5f,0); 
    }
    
    public static Vector3 ToTilePos(Vector2 vector2)
    {
        return ToPointPos(vector2) + new Vector3(0.5f, 0.5f,0); 
    }
}


public class UiTool
{
    private static bool isLoop;
    public static void BreathSprite(SpriteRenderer sprite,float minAlpha,float maxAlpha,float loopTime)
    {
            
    }

    
//    private IEnumerator StartBreath(SpriteRenderer sprite, float minAlpha, float maxAlpha, float loopTime,bool isLoop)
//    {
//        float countTime = loopTime;
//        float eachTime = 0.02f;
//        Color color = sprite.color;
//        sprite.color = ColorTool.ChangeAlpha(color, minAlpha);
//        do
//        {
//            
//        } while (countTime>=0);
//        
//        sprite.color = ColorTool.ChangeAlpha(color, maxAlpha);
//    }

    public static Vector2 GetTowerUiPos(Transform transform,Camera UiCamera, Vector3 pos)
    {
        //使用场景相机将世界坐标转换为屏幕坐标
        Vector2 screenUiPos = UiCamera.WorldToScreenPoint(pos);
            
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, 
            screenUiPos,
            UiCamera, 
            out Vector2 retPos);
        return retPos;
    }
    public static Vector2 GetTowerUiPos(Transform transform,Camera UiCamera, Point point)
    {
        return GetTowerUiPos(transform, UiCamera, point.CenterPos);
    }
}
public class Swap
{
    public static void ValueType<T>(ref T a,ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    public static void ReferenceType<T>(T a,T b)
    {
        ValueType(ref a, ref b);
    }
}

public class GetDir
{
     int dir;               //因为要保留上次的dir
    public  int Dir(float dir)
    {
        if (dir > 0)
        {
            this.dir = 1;
        }
        else if(dir<0)
        {
            this.dir = -1;
        }
        return this.dir;
    }
}

public class ColorTool
{
    static List<float> arr = new List<float>();
    public static Color RandomColor(float max = 1,float min = 0.7f,float alpha=0.7f)
    {
        arr = new List<float>();
        float random = Random.Range(min, max);
        Color randomColor;
        arr.Add(max);
        arr.Add(min);
        arr.Add(random);
        randomColor = Color.white;
        randomColor.a = alpha;
        for (int i = arr.Count - 1; i >= 0; i--)
        {
            int num = Random.Range(0, arr.Count);
            if (i == 0)
                randomColor.r = arr[num];
            else if (i == 1)
                randomColor.g = arr[num];
            else if (i == 2)
                randomColor.b = arr[num];
            arr.RemoveAt(num);
        }
        return randomColor;
    }

    public static Color ChangeAlpha(Color color, float alpha)
    {
        color = new Color(color.r,color.g,color.b,alpha);
        return color;
    }
   
}

public class CompareTool
{
    public static bool DisLongerThan(Transform a,Transform b,float dis)
    {
        return DisLongerThan(a.position, b.position, dis);

    }
    
    public static bool DisLongerThan(Vector3 a,Vector3 b,float dis)
    {
        return ((a - b).sqrMagnitude > dis * dis);

    }
}

public class TimeTool
    {
        public static void LongToTime(long num,out int days,out int hours,out int mins,out int seconds)
        {
            int time = (int)num;
            days = hours = mins = seconds = 0;
            days = time / 86400;
            time = time - 86400 * days;
            hours = time / 3600;
            time = time - 3600 * hours;
            mins = time / 60;
            time = time - 60 * mins;
            seconds = time;
        }
        
        
        public static void MsToMS(int totleMs,out int mins,out int seconds,out int ms)
        {
            mins = seconds = ms = 0;

            seconds = totleMs / 1000;
            mins = seconds / 60;
            ms = totleMs - seconds * 1000;
        }
        
        public static long RefreshTime(int updateTime = 5)
        {
            updateTime = updateTime % 24;
            
            DateTime checkDateTime = DateTime.Today.AddHours(updateTime);
            long time = (long)(checkDateTime - DateTime.Now).TotalSeconds;
            
            //今天5点之后
            if(time < 0)
            {
                //明天5点
                checkDateTime = DateTime.Today.AddHours(24 + updateTime);
                time = (long)(checkDateTime - DateTime.Now).TotalSeconds;
            }
            return time;
        }
        
        public static string ToTimeStr(long timetext)
        {
            LongToTime(timetext, out int days, out int hours, out int mins, out int seconds);
            
            if (days > 0)
            {
                return string.Format("{0:D2}{1} {2:D2}{3}", days,"d", hours, "h");
            }
            if (hours > 0)
            {
                return string.Format("{0:D2}{1} {2:D2}{3}", hours,"d", mins, "m");
            }
            if (mins > 0)
            {
                return string.Format("{0:D2}{1} {2:D2}{3}", mins,"m", seconds, "s");
            }
            return string.Format("{0:D2}{1}", seconds, "s");
        }
        
        public static string ToTimeStrSimple(long timetext)
        {
            LongToTime(timetext, out int days, out int hours, out int mins, out int seconds);
            
            if (days > 0)
            {
                return string.Format("{0:D2}:{1:D2}", days, hours);
            }
            if (hours > 0)
            {
                return string.Format("{0:D2}:{1:D2}", hours,mins);
            }
            if (mins > 0)
            {
                return string.Format("{0:D2}:{1:D2}", mins,seconds);
            }
            return string.Format("{0:D2}{1}", seconds, "s");
        }
        
        public static string MsToTimeStrSimple(int totleMs)
        {
            MsToMS(totleMs,  out int mins, out int seconds,out int ms);
            
            if (mins > 0)
            {
                return string.Format("{0:D2}:{1:D2}",mins,seconds);
            }
            if (seconds > 0)
            {
                return string.Format("{0:D2}:{1:D2}", seconds,ms/10);
            }
            return string.Format("{0:D2}{1}", ms/10, "ms");
        }
        
        public static string MsToTimeStr(int totleMs)
        {
            MsToMS(totleMs,  out int mins, out int seconds,out int ms);
            
            if (mins > 0)
            {
                return string.Format("{0:D2}{1}:{2:D2}{3}", mins,"m", seconds, "s");
            }
            if (seconds > 0)
            {
                return string.Format("{0:D2}{1}:{2:D2}{3}", seconds,"s", ms/10, "ms");
            }
            return string.Format("{0:D2}{1}", ms/10, "ms");
        }
        
}

