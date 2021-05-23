using System.Collections;
using System.Collections.Generic;
using mapThing;
using UnityEngine;

namespace level
{
    public class LevelMgr : MonoBehaviour
    {
        public int levelIndex;

        public LevelMsg levelMsg => AssestMgr.Instance.LoadLevelMsg(levelIndex);

        public Dictionary<int, GameObject> spawnPoint;

        void Start()
        {
            spawnPoint = new Dictionary<int, GameObject>();
            Map.Instance.Init(levelIndex);
        }


        private void Init()
        {
            List<Point> startPoints = GameManager.startPoints;
            for (int i =0;i< startPoints.Count;i++)
            {
                GameObject start = new GameObject($"start{i}");
                start.transform.position = startPoints[i].CenterPos;
            }
        }
        void Update()
        {
            
        }
    }
    

} 
