using System;
using System.Collections;
using System.Collections.Generic;
using mapThing;
using Player;
using UnityEngine;

namespace tower
{
    public class BaseTower : MonoBehaviour
    {
        public TowerType type;
        public TileType requireTile;
        
        [HideInInspector]
        public Tower tower;
        protected TowerMsg curTower;
        public TowerMsg CurTower
        {
            get
            {
                if (curTower == null)
                    return tower.towerMsg;
                return curTower;
            }
        }
        public Point CurPoint => GameManager.GetPointByPos(transform.position);
        
        
        [HideInInspector]
        public TowerState state;
        protected SpriteRenderer sp;
        
        private int index;
        
        
        public virtual void Init()
        {
            state = TowerState.Idle;
            tower = AssestMgr.Instance.GetTower(type);
            sp = transform.GetComponent<SpriteRenderer>();
        }
     
        public virtual bool CheckCanUpGrade(int index)
        {
            if(CurTower.nextLevelTower.Length <= index)
                return false;

            if (PlayerStateMgr.Instance.player.coinCount < CurTower.nextLevelTower[index].buildPrice)
                return false;

            this.index = index;
            return true;
        }


        public virtual void OnBuilding()
        {
            state = TowerState.Upgrading;
            PlayerStateMgr.Instance.player.coinCount -= CurTower.nextLevelTower[index].buildPrice;
        }
        public virtual void OnBuildOver()
        {
            state = TowerState.Idle;
            curTower = CurTower.nextLevelTower[index];
            
        }
        public void Seal()
        {
                
        }
        public virtual BaseTower PickTower(Transform player)
        {
            Point point = CurPoint;
            transform.SetParent(player);
            transform.localPosition = Vector3.zero;
            point.RemoveTower();
            GameManager.ChangeGameState(GameState.PickTower);
            return this;
        }

        public virtual void PutPower()
        {
            Point point = CurPoint;
            sp.sortingOrder = point.Y;
            if (point.HadTower && point.type == TowerType.UpGrade)
            {
                UpGradeTower upGradeTower = point.BaseTower as UpGradeTower; 
                upGradeTower.PrepareUpgreade(this);
                transform.SetParent(upGradeTower.transform);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.SetParent(null);
                transform.position = point.CenterPos;
                point.SetTower(this);
                GameManager.ChangeGameState(GameState.PlayGame);
            }
            
            
            
            
        }
        
    }
    

}
