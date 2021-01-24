using System;
using System.Collections;
using System.Collections.Generic;
using Data.Util;
using Enemy;
using UnityEngine;


public class BaseEnemy : BaseProperty,ITakeDamage
{
    protected BoxCollider2D box;
    protected Rigidbody2D rb;
    protected BaseEnemyAnimStateMgr animStateMgr;
    
    protected override void Start()
    {
        base.Start();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();
    }
    
    public virtual void TakeDamage(int damage)
    {
        _curHp -= damage;
        HpBarUpDate();
        if(_curHp <=0)
            Dead();    
    }
    
}
