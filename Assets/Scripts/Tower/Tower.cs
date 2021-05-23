using System;
using mapThing;
using UnityEngine;

namespace tower
{
    [CreateAssetMenu(fileName = "tower", menuName = "Tower", order = 1)]
    public class Tower : ScriptableObject
    {
        public TowerType towerType;
        public TileType tileType;
        public TowerMsg towerMsg;
        [NonSerialized]
        public int maxLevel;
    }

    [Serializable]
    public class TowerMsg
    {
        public int atk;
        public int buildPrice;
        public float buildTime;
        public Sprite towerSprite;
        public TowerMsg[] nextLevelTower;
        [NonSerialized]
        public int curLevel;
    }

}
