using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;


public class BaseEnemy : BaseProperty,ITakeDamage
{
    
    public int RodeID;
    protected BoxCollider2D box;
    protected BaseEnemyAnimStateMgr animStateMgr;
    
    protected Queue<Transform> pathQueue = new Queue<Transform>();
    

    protected Transform target;
    protected Transform wayPoint;
    protected Transform enemyTarget;
    
    protected override void Start()
    {
        base.Start();
        
        box = GetComponent<BoxCollider2D>();
        animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();

        
        pathQueue = new Queue<Transform>(TDRode.GetPathList(RodeID));
        if (pathQueue == null || pathQueue.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = pathQueue.Dequeue().position;
        wayPoint = pathQueue.Dequeue();
        target = wayPoint;
    }
    

    protected virtual void Arrived()
    {
        animStateMgr.ChangeState(EnemyState.Dead);
    }

    public virtual void TakeDamage(int damage)
    {
        _curHp -= damage;
        HpBarUpDate();
        if(_curHp <=0)
            Dead();    
    }

    public virtual void FaceToTarget()
    {
        if (transform.position.x - target.position.x > 0)
            transform.localScale = new Vector3(-1,1,1);
        else
            transform.localScale = Vector3.one;
        
    }
    
    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
    }
    
}
