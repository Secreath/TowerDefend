using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Enemy;
using tower;
using UnityEngine;

namespace level
{
    [CreateAssetMenu(fileName = "LevelMsg", menuName = "LevelMsg", order = 1)]
    public class LevelMsg : ScriptableObject
    {
        public int levelIndex;
        public string chooseTowers;
        public List<EachWave> enemySpawnWave;
    }



    [Serializable]
    public class EachWave
    {
        public WaveTime waveTime;
        public List<SingleWavePointMsg> wavePoint;
    }
    
    [Serializable]
    public class WaveTime
    {
        public int restTime;
        public int attactTime;
    }

    [Serializable]
    public class SingleWavePointMsg
    {
        public int pointId;
        public int pathId;
        public List<EnemyBornMsg> EnemyBornMsgs;
    }
    
    [Serializable]
    public class EnemyBornMsg
    {
        public float waitTime;
        public float intervalTime;
        
        public List<EnemyDetail> enemys;
    }

    [Serializable]
    public class EnemyDetail
    {
        public float num;
        public EnemyType type;
    }

}
