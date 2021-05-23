using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;


public class BaseEnemy : BaseProperty,ITakeDamage
{
    public Point CurPoint => GameManager.GetPointByPos(transform.position);
    
    public int RoadId;
    protected BoxCollider2D box;
    protected BaseEnemyAnimStateMgr animStateMgr;
    
    protected Queue<Point> pathQueue = new Queue<Point>();
    

    protected Point target;
    protected Point wayPoint;

    protected bool start;
    //protected Point enemyTarget;
    
    protected override void Start()
    {
        base.Start();
        
        box = GetComponent<BoxCollider2D>();
        animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();

        EventCenter.GetInstance().AddEventListener("GameManagerInit",Init); 
        
    }

    public void Init()
    {
        pathQueue = new Queue<Point>(GameManager.GetPointList(RoadId));
        
        if (pathQueue == null || pathQueue.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log(pathQueue.Count);
        transform.position = pathQueue.Dequeue().CenterPos;
        wayPoint = pathQueue.Dequeue();
        target = wayPoint;
        start = true;
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
        if (transform.position.x - target.CenterPos.x > 0)
            transform.localScale = new Vector3(-1,1,1);
        else
            transform.localScale = Vector3.one;
        
    }
    
    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
    }
    
}
