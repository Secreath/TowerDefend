using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }
}


//public abstract class SingletonWithOutMono<T>
//{
//    private static T instance;
//
//    public static T Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                instance = new T();
//            }
//
//            return instance;
//        }
//    }
//}