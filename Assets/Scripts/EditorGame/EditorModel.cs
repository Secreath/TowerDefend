using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SaveRes
{
    public class EditorModel : MonoBehaviour
    {
        public string resPath;
        public string fileName;

        public string localPath => $"{Application.dataPath}/Resources/{resPath}";
        public string fileFullName => $"{fileName}.json";

        public string filePath => $"{localPath}/{fileFullName}";
        
        private Button save;
        void Start()
        {
            if(transform.Find("SavePathBtn") == null)
                return;
            save = transform.Find("SavePathBtn").GetComponent<Button>();
            save.onClick.AddListener(Save);
        }
     
        void Save()
        {
            Debug.Log(CheckFile());
            switch (CheckFile())
            {
                case FileExsitStatus.NoPath:
                    System.IO.Directory.CreateDirectory(localPath);//不存在就创建文件夹
                    CreatFile();
                    break;
                case FileExsitStatus.NoFile:
                    CreatFile();
                    break;
                case FileExsitStatus.FileExsit:
                    ConvertPathToStr();
                    break;
            }
         
        }
        
        private void CreatFile()
        {
            string filePath = System.IO.Path.Combine(localPath, fileFullName);
            System.IO.File.Create(filePath);
        }
        
        private FileExsitStatus CheckFile()
        {
            if (string.IsNullOrEmpty(localPath))
            {
                return FileExsitStatus.NoPath;
            }
            
            //System.IO.Directory.CreateDirectory(@"E:\Files");//不存在就创建文件夹
            if (!System.IO.Directory.Exists(localPath)) 
            { 
                return FileExsitStatus.NoPath;
            }
            
            
            if(File.Exists(filePath)) 
            { 
                //存在
                return FileExsitStatus.FileExsit;
            } 
            else 
            { 
                //不存在 
                return FileExsitStatus.NoFile;
            } 
                
        }
        
      
        
        void AppendOrOPenFile(string path, string name, string info) //路径、文件名、写入内容
        {          
            StreamWriter sw; 
            FileInfo fi = new FileInfo(path + "/" + name);
            sw = fi.AppendText();        //直接重新写入，如果要在原文件后面追加内容，应用fi.AppendText()
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
            Debug.Log("AppendFile");
        }


        void ConvertPathToStr()
        {
            LevelPathData levelPathData = new LevelPathData();

            levelPathData.level = 1;
            List<PathData> pathData = new List<PathData>();

            List<List<Transform>> roadList = Road.Instance.roadList;
            for(int i =0;i< roadList.Count ;i++)
            {
                PathData data = new PathData();
                data.id = i;
                data.pathList = TransPath(roadList[i]);
                pathData.Add(data);
            }

            levelPathData.PathDatas = pathData;
            
            string jsonstring = JsonUtility.ToJson(levelPathData);
            Debug.Log(jsonstring);
            CreateOrOPenFile(localPath, fileFullName, jsonstring);
        }

        void CreateOrOPenFile(string path, string name, string info) //路径、文件名、写入内容
        {          
            StreamWriter sw; 
            FileInfo fi = new FileInfo(path + "/" + name);
            sw = fi.CreateText ();        //直接重新写入，如果要在原文件后面追加内容，应用fi.AppendText()
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
            Debug.Log("writeFile");
        }
        
        List<Vector2> TransPath(List<Transform> pathTrans)
        {
            List<Vector2> posList = new List<Vector2>();
            for (int i = 0; i < pathTrans.Count; i++)
            {
                posList.Add(VTool.ToTilePos(pathTrans[i].position));
            }

            return posList;
        }
    }

    
    [Serializable]
    public class LevelPathData
    {
        public int level;
        public List<PathData> PathDatas;

        public LevelPathData()
        {
            PathDatas = new List<PathData>();
        }

    }
    [Serializable]
    public class PathData
    {
        public int id;
        public List<Vector2> pathList;
    }

}
