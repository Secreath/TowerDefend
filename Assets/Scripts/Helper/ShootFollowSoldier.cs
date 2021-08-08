using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Follower;
using Player;
using UnityEngine;

namespace solider
{
    public class ShootFollowSoldier : FollowSoldier,IAttack
    {
        public virtual void Attack()
        {
            if(_enemy == null)
                return;
            
            GameObject bullet = PoolMgr.GetInstance().PopObj("PeaBall");
            bullet.tag = "PlayerBullet";
            bullet.GetComponent<BaseBullet>().SetShotDir(_curProperty.atk, _enemy.transform.position, transform.position);
             
        }
    }
    
}
