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
        public Tower tower;
        
        public TowerType type => tower.towerType;
        public TileType tileType => tower.tileType;
        
        
        private TowerMsg curTower;
        public TowerMsg CurTower
        {
            get
            {
                return curTower;
            }
        }
        
        public TowerMsg NextTower
        {
            get
            {
                if (curTower.nextLevelTower.Length > index)
                    return curTower.nextLevelTower[index];
                return null;
            }
        }
        
        public Point CurPoint => GameManager.GetPointByPos(transform.position);
        
        
        [HideInInspector]
        public TowerState state;
        
        protected SpriteRenderer sp;
        protected BoxCollider2D box;
        protected Animator anim;
        
        protected int index;
        protected virtual void Start()
        {
            sp = transform.GetComponent<SpriteRenderer>();
            box = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
            Init();
        }
        
        
        public virtual void Init()
        {
            index = 0;
            state = TowerState.Idle;
            curTower = tower.towerMsg;

        }
     
        
        public virtual bool CheckCanUpGrade(int index)
        {
            if(CurTower.nextLevelTower.Length <= index)
                return false;

            if (PlayerPackage.Instance.CoinNums < CurTower.nextLevelTower[index].buildPrice)
                return false;
            this.index = index;
            return true;
        }


        public virtual void OnBuilding()
        {
            state = TowerState.Upgrading;
            PlayerPackage.Instance.CoinNums -= CurTower.nextLevelTower[index].buildPrice;
            
        }
        public virtual void OnBuildOver()
        {
            state = TowerState.Idle;
            curTower = CurTower.nextLevelTower[index];
            anim.SetInteger("curLevel",curTower.curLevel);
        }
        public void Seal()
        {
                
        }
        public virtual BaseTower PickTower(Transform player)
        {
            Point point = CurPoint;
            if (point.towerType == TowerType.UpGrade)
            {
                state = TowerState.Idle;
                UpGradeTower upGradeTower = point.BaseTower as UpGradeTower;
                return upGradeTower.PickTower(player);
            }
            else
            {
                state = TowerState.OnPick;
                transform.SetParent(player);
                transform.localPosition = Vector3.zero;
                point.RemoveTower();
                GameManager.ChangeGameState(GameState.PickTower);
                return this;
            }
        }

        public virtual void PutPower()
        {
            Point point = CurPoint;
            sp.sortingOrder = point.Y;
            if (point.towerType == TowerType.UpGrade)
            {
                UpGradeTower upGradeTower = point.BaseTower as UpGradeTower; 
                upGradeTower.PrepareUpgreade(this);
                transform.SetParent(upGradeTower.transform);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.Log(name + " " + CurTower.curLevel);
                state = TowerState.Idle;
                transform.SetParent(null);
                transform.position = point.CenterPos;
                point.SetTower(this);
                GameManager.ChangeGameState(GameState.PlayGame);
            }
            
            
            
            
        }
        
    }
    

}
