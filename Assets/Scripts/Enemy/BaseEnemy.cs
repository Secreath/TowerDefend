using System;
using System.Collections;
using System.Collections.Generic;
using Data.Util;
using Enemy;
using UnityEngine;


public class BaseEnemy : BaseProperty,ITakeDamage
{
    protected EnemyState enemyState;
    protected BoxCollider2D box;
    protected BaseEnemyAnimStateMgr animStateMgr;
    
    protected override void Start()
    {
        base.Start();
        enemyState = EnemyState.Walk;
        box = GetComponent<BoxCollider2D>();
        animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();
        animStateMgr.TryChangeState(EnemyState.Walk);
    }
    
    public virtual void TakeDamage(int damage)
    {
        _curHp -= damage;
        HpBarUpDate();
        if(_curHp <=0)
            Dead();    
    }
    
}
