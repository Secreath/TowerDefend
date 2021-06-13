using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Enemy;
using mapThing;
using SaveRes;
using ui;
using UnityEngine;

namespace level
{
    public class LevelMgr : Singleton<LevelMgr>
    {
        public int levelIndex;

        public bool EnemyBorn;
        private LevelMsg levelMsg;
        private LevelPathData levelPathData;
        
        public Dictionary<int, List<Point>> pathDic;
        public Dictionary<int, BornPoint> spawnPointDic;

        public struct BornPoint
        {
            public Transform trans;
            public Point point;

            public BornPoint(Transform trans, Point point)
            {
                this.trans = trans;
                this.point = point;
            }
        }
        public List<GameObject> EnemyList;
        private int enemyIndex;
        void Start()
        {
            spawnPointDic = new Dictionary<int, BornPoint>();
            gameObject.AddComponent<AssestMgr>();
            Map.Instance.Init(levelIndex);
        }


        public void Init()
        {
            List<Point> startPoints = GameManager.Instance.startPoints;
            for (int i =0;i< startPoints.Count;i++)
            {
                GameObject start = new GameObject($"start{i}");
                start.transform.parent = transform;
                start.transform.position = startPoints[i].CenterPos;
                
                BornPoint bornPoint = new BornPoint(start.transform,startPoints[i]);
                spawnPointDic.Add(i,bornPoint);
            }

            SetLevelMsg();
        }

        public void SetLevelMsg()
        {
            pathDic = new Dictionary<int, List<Point>>();
            
            levelMsg = AssestMgr.Instance.CurLevelMsg;
            levelPathData = AssestMgr.Instance.LoadLevelPathData(levelIndex);

            for (int i = 0; i < levelPathData.PathDatas.Count; i++)
            {
                List<Vector2> vec2Lsit = levelPathData.PathDatas[i].pathList;
                pathDic.Add(levelPathData.PathDatas[i].id,GameManager.GetPointsByPos(vec2Lsit));
            }

            if(EnemyBorn)
                StartCoroutine(StartGames());
        }

        private bool isRest;
        public IEnumerator StartGames()
        {
            for (int i = 0; i < levelMsg.enemySpawnWave.Count; i++)
            {
                List<SingleWavePointMsg> wavePoint = levelMsg.enemySpawnWave[i].wavePoint;
                WaveTime waveTime = levelMsg.enemySpawnWave[i].waveTime;
                isRest = true;
                Debug.Log($"rest {waveTime.restTime}");
                
                //UiManager.Instance.ShowTimeCountPanel(waveTime.restTime,$"敌人即将进攻 做好准备");
                
                yield return new WaitForSeconds(waveTime.restTime);
                isRest = false;
                for (int j = 0; j < wavePoint.Count; j++)
                {
                    StartCoroutine(EachBornPoint(wavePoint[j]));
                }
                Debug.Log($"attack {waveTime.attactTime}");
                yield return new WaitForSeconds(waveTime.attactTime);
            }
        }

        public IEnumerator EachBornPoint(SingleWavePointMsg bornPoint)
        {
            for (int j = 0; j < bornPoint.EnemyBornMsgs.Count; j++)
            {
                if(isRest)
                    yield break;
                
                EnemyBornMsg bornMsg = bornPoint.EnemyBornMsgs[j];
                Debug.Log($"{j}  watiTime {bornMsg.waitTime}");
                yield return new WaitForSeconds(bornMsg.waitTime);
                
//                StartCoroutine(EachEnemyWave(bornMsg, bornPoint.pointId, bornPoint.pathId));

                int bornPathID = bornPoint.pathId;
                int bornPointID = bornPoint.pointId;
                if(bornMsg.enemys.Count < 1)
                    yield break;
                if (bornMsg.enemys.Count == 1)
                {
                    GameObject enemy = AssestMgr.Instance.LoadEnemy(bornMsg.enemys[0].type);
                    BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
                    baseEnemy.RoadId = bornPathID;
                    for (int i = 0; i < bornMsg.enemys[0].num; i++)
                    {
                        InstantiateEnemy(bornMsg.enemys[0].type, bornPointID, bornPathID);
                    
                        Debug.Log($"1  intervalTime {bornMsg.intervalTime}");
                        yield return new WaitForSeconds(bornMsg.intervalTime);
                    }
                }
                else
                {
                    for (int i = 0; i < bornMsg.enemys.Count; i++)
                    {
                        EnemyDetail enemy = bornMsg.enemys[i];
                        StartCoroutine(YieldInstantiateEnemy(enemy.type,enemy.num, bornPointID, bornPathID));
                        Debug.Log($"2  intervalTime {bornMsg.intervalTime}");
                        yield return new WaitForSeconds(bornMsg.intervalTime);
                    }
                }

            }
        }

        public IEnumerator EachEnemyWave(EnemyBornMsg bornMsg, int bornPointID, int bornPathID)
        {
            if(bornMsg.enemys.Count < 1)
                yield break;
            if (bornMsg.enemys.Count == 1)
            {
                GameObject enemy = AssestMgr.Instance.LoadEnemy(bornMsg.enemys[0].type);
                BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
                baseEnemy.RoadId = bornPathID;
                for (int i = 0; i < bornMsg.enemys[0].num; i++)
                {
                    InstantiateEnemy(bornMsg.enemys[0].type, bornPointID, bornPathID);
                    
                    Debug.Log($"1  intervalTime {bornMsg.intervalTime}");
                    yield return new WaitForSeconds(bornMsg.intervalTime);
                }
            }
            else
            {
                for (int i = 0; i < bornMsg.enemys.Count; i++)
                {
                    EnemyDetail enemy = bornMsg.enemys[i];
                    StartCoroutine(YieldInstantiateEnemy(enemy.type,enemy.num, bornPointID, bornPathID));
                    Debug.Log($"2  intervalTime {bornMsg.intervalTime}");
                    yield return new WaitForSeconds(bornMsg.intervalTime);
                }
            }
            
        }

        public IEnumerator YieldInstantiateEnemy(EnemyType type, float waitTime, int pointID, int pathID)
        {
            InstantiateEnemy(type, pointID, pathID);
            yield return new WaitForSeconds(waitTime);
        }

        public void InstantiateEnemy(EnemyType type,int pointID, int pathID)
        {
            GameObject go = AssestMgr.Instance.LoadEnemy(type);
            GameObject enemy = Instantiate(go, spawnPointDic[pointID].trans);
            BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
            baseEnemy.RoadId = pathID;
            enemy.transform.position = spawnPointDic[pointID].trans.position;
            enemy.name = $"Enemy{enemyIndex++}";
            EnemyList.Add(enemy);
        }
    }
    

} 
