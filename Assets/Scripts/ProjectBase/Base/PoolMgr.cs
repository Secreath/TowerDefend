using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//缓存池模块
//Dictionary List
//GameObject和Resource两个公共类中的API
public class PoolData
{
    public bool isDynamic;
    public GameObject fatherObj;
    public List<GameObject> poolList;
    //对象,父对象,是否动态
    public PoolData(bool isDynamic, GameObject obj, GameObject poolObj)
    {
        //将父节点设为obj
        this.isDynamic = isDynamic;
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() { obj };
        PushObj(obj);
    }
    //进栈
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        if (isDynamic)
        {
            poolList.Add(obj);
        }      
        obj.transform.parent = fatherObj.transform;
    }
    //出栈
    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        if (!isDynamic)                         //静态池直接放入列表末端
            poolList.Add(obj);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
    public void InitStaticPool(int Count)
    {
        if (!isDynamic && poolList[0]!=null)
        {            
            for (int i = 1; i < Count; i++)
            {                
                
                poolList.Add(MonoMgr.GetInstance().CopyGameObject(poolList[0],fatherObj.transform));
            }
            Debug.Log(Count);
        }
        else
        {
            Debug.Log("创建失败");
        }
    }
    public void Clear()
    {
        poolList.Clear();
    }
}
public class PoolMgr : MonoBehaviour
{
    static PoolMgr instance;

    public Dictionary<string, PoolData> dynamicPoolDic = new Dictionary<string, PoolData>();        //动态池
    public Dictionary<string, PoolData> staticPoolDic = new Dictionary<string, PoolData>();             //静态池
    
    //private GameObject poolObj = new GameObject("pool");

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public static PoolMgr GetInstance()
    {
        return instance;
    }

    //手动创建一个对象池 名字  长度 和 obj
    public void InitStaticPool(GameObject obj,int length)
    {
        if (!staticPoolDic.ContainsKey(obj.name) && !dynamicPoolDic.ContainsKey(obj.name))
        {
            
            staticPoolDic.Add(obj.name, new PoolData(false, obj, gameObject));
            staticPoolDic[obj.name].InitStaticPool(length);
        }
        else
            Debug.Log("已经创建过该池");
    }

    public void GetSObj(string name, UnityAction<GameObject> callBack)
    {
        if (dynamicPoolDic.ContainsKey(name))
        {
            Debug.Log("该对象属于动态池");
            return;
        }
        if (staticPoolDic.ContainsKey(name) && staticPoolDic[name].poolList.Count > 0)
        {
            callBack(staticPoolDic[name].GetObj());
        }
        else
        {
            Debug.Log("该静态池中没有目标");
        }
    }

    public void GetDObj(string name,string path, UnityAction<GameObject> callBack)
    {
        if (staticPoolDic.ContainsKey(name))
        {
            Debug.Log("该对象属于静态池");
            return;
        }
        if (dynamicPoolDic.ContainsKey(name) && dynamicPoolDic[name].poolList.Count > 0)
        {
            callBack(dynamicPoolDic[name].GetObj());        
        }
        else
        {
            //通过异步加载资源 如果池中没有
            ResMgr.GetInstance().LoadAsync<GameObject>(path, (o) =>
            {
                o.name = name;
                callBack(o);
            });   
        }  
    }
    //放入
    public void PushObj(GameObject obj,UnityAction<GameObject> action = null)
    {
        //if (poolObj == null)
        //{
        //    poolObj = new GameObject("Pool");
        //}
        if (action != null)
            action(obj);
        obj.transform.position = gameObject.transform.position; 
        obj.transform.parent = gameObject.transform;
        //在这里移除组件
        

        //obj.SetActive(false);
        //有抽屉
        if (dynamicPoolDic.ContainsKey(obj.name))
        {
            dynamicPoolDic[obj.name].PushObj(obj);
        }
        else if (staticPoolDic.ContainsKey(obj.name))
        {
            staticPoolDic[obj.name].PushObj(obj);
        }
        //无抽屉
        else
        {
            dynamicPoolDic.Add(obj.name, new PoolData(true,obj, gameObject));
        }
    }

   

    //清空缓存池方法
    public void Clear()
    {
        dynamicPoolDic.Clear();
        staticPoolDic.Clear();
    }
}
