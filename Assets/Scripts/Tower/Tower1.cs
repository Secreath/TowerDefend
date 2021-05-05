using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tower
{
    public class Tower1 : BaseTower
    {
        public float range;
        public float attackTime;
        
        private TowerRange towerRange;
        private Animator anim;
        private GameObject attackTarget;
        private Vector2 shotPoint;

        private bool attacked;

        
        private Vector3 targetPos;
        void Start()
        {

            anim = GetComponent<Animator>();
            towerRange = transform.Find("range").GetComponent<TowerRange>();
            shotPoint = transform.Find("ShotPoint").position;
            towerRange.SetRange(range,attackTime);
        }

        
        public void Attack(Vector3 targetPos)
        {
            if(attacked)
                return;
            attacked = true;
            Invoke(nameof(Attacked),attackTime);
            Debug.Log("Att");
            this.targetPos = targetPos;
            anim.SetBool("isAttack",true);       
            
            
        }


       

        public void Shoot()
        {
            GameObject bullet = PoolMgr.GetInstance().PopObj("PeaBall");
            bullet.tag = "PlayerBullet";
            bullet.GetComponent<BaseBullet>().SetShootDir(10, targetPos, shotPoint);
        }

        public void ResetAttack()
        {
            anim.SetBool("isAttack",false);
        }


        private void Attacked()
        {
            attacked = false;
        }
        
        
        
    }

}
