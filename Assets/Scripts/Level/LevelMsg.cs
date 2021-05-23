using System;
using System.Collections;
using System.Collections.Generic;
using tower;
using UnityEngine;

namespace level
{
    [CreateAssetMenu(fileName = "LevelMsg", menuName = "LevelMsg", order = 1)]
    public class LevelMsg : ScriptableObject
    {
        public int levelIndex;
        public string chooseTowers;
        public List<EachWaveTime> spawnTimeList;
        public List<EnemySpawnPoint> spawnPointList;
    }



    [Serializable]
    public class EnemySpawnPoint
    {
        public int index;
        public List<EachWaveEnemy> enemySpawnList;
    }

    [Serializable]
    public class EachWaveTime
    {
        public int restTime;
        public int attakTime;
    }
    [Serializable]
    public class EachWaveEnemy
    {
        public bool isSpawn;
        public int path;
        public float eachBorn;
        public List<BaseEnemy> EnemyList;
    }
    

}
