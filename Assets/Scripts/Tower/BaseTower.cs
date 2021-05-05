using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tower
{
    public class BaseTower : MonoBehaviour
    {
        public Tower tower;
        public int level;
        public TowerMsg curTower;
        public List<int> upgradePrice = new List<int>();

        protected int curLevel;
        public int CurLevel => curLevel;

        public int CurUpgradePrice
        {
            get
            {
                if (curLevel < upgradePrice.Count)
                    return upgradePrice[CurLevel];
                return 0;
            }
        }

        public void Start()
        {
            curLevel = 0;
        }

        public void UpGrade()
        {
            Debug.Log("UpGrade");
        }
            
        public void Seal()
        {
                
        }
    }
    

}
