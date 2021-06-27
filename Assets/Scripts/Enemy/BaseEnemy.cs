using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;


public class BaseEnemy : BaseProperty,ITakeDamage
{
    public Point CurPoint => GameManager.GetPointByPos(transform.position);
    
    public EnemyType type;
    public int RoadId;
    public float attackRadius;
    
    protected BoxCollider2D box;
    protected BaseEnemyAnimStateMgr animStateMgr;
    protected EnemyState enemyState;
    
    protected Queue<Point> pathQueue = new Queue<Point>();

    private Transform attackTarget;
    protected Point wayPoint;

    protected bool start;
    //protected Point enemyTarget;
    
    protected override void Start()
    {
        base.Start();
        
        box = GetComponent<BoxCollider2D>();
        animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();

//        EventCenter.GetInstance().AddEventListener("GameStart",Init);
        Init();
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
        
        start = true;
    }


    public void FaceToTarget()
    {
        Vector3 targetPos;
        if (attackTarget != null)
            targetPos = attackTarget.position;
        else
            targetPos = wayPoint.CenterPos;
        
        if (transform.position.x - targetPos.x > 0)
            transform.localScale = new Vector3(-1,1,1);
        else
            transform.localScale = Vector3.one;
        
    }
    public void TauntEnemy(Transform tauntTarget,int hashCode)
    {
        if (attackTarget == null)
        {
            Debug.Log(gameObject.name);
            EventCenter.GetInstance().AddEventListener($"{hashCode}Destory",ClearAttackTarget,true);
            this.attackTarget = tauntTarget;
        }

        void ClearAttackTarget()
        {
            Debug.Log("ClearAttack");
            this.attackTarget = null;
            EventCenter.GetInstance().RemoveEventListener($"{hashCode}Destory",ClearAttackTarget);
        }
    }
    
    public virtual void TakeDamage(int damage)
    {
        _curHp -= damage;
        HpBarUpDate();
        if (_curHp <= 0)
            EnemyDead();
    }
    
    
    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void CheckWayPoint()
    {
        if(!CompareTool.DisLongerThan(transform.position,wayPoint.CenterPos,0.1f))
        {
            if (pathQueue.Count < 1)
            {
                //Arrived
                animStateMgr.ChangeState(EnemyState.Dead);
                EnemyDead();
            }
            else
            {
                wayPoint = pathQueue.Dequeue();
            }
        }
        
       

    }
    
    private void Update()
    {
        CheckWayPoint();
        SetUiPos(transform.position);
        if(!start)
            return;
        CheckAround();
        CheckMoveState();
        ChangeMoveState();
        FaceToTarget();
    }

    private void CheckMoveState()
    {
        switch (enemyState)
        {
            case EnemyState.Dead:
                EnemyDead();
                break;
            case EnemyState.WalkToEnemy:
                EnemyWalkToEnemy();
                break;
            case EnemyState.Attack:
                EnemyAttack();
                break;
            case EnemyState.Walk:
                EnemyWalk();
                break;
            case EnemyState.Idle:
                EnemyIdle();
                break;
        }
    }

    public void ChangeMoveState()
    {
        if (_curHp <= 0)
            enemyState = EnemyState.Dead;
        else if(attackTarget!=null && CompareTool.DisLongerThan(transform,attackTarget,attackRadius))
            enemyState = EnemyState.WalkToEnemy;
        else if(attackTarget!=null)
            enemyState = EnemyState.Attack;
        else if (wayPoint != null)
        {
            enemyState = EnemyState.Walk;
        }
        else if (attackTarget == null && wayPoint == null)
        {
            enemyState = EnemyState.Idle;
        }
    }

    void CheckAround()
    {
//        checkPoint = (Vector2)transform.position + box.offset;
//        Collider2D enemyCircle = Physics2D.OverlapCircle(checkPoint, checkRadius,LayerMask.GetMask("Block"));
//            
//        if (enemyCircle != null)
//        {
//            target = GameManager.GetPointByPos(enemyCircle.transform.position);
//            AttackModel(enemyCircle.transform);
//        }
//        else
//        {
//            target = wayPoint;
//            if(animStateMgr.CurState() != EnemyState.Walk)
//                StartCoroutine(MoveToEndPoint());
//        }
    }
    
    private void EnemyDead()
    {
        animStateMgr.ChangeState(EnemyState.Dead);
        Dead();
    }
    
    private void EnemyWalkToEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            attackTarget.CenterPos(), curSpeed* Time.deltaTime);
        animStateMgr.ChangeState(EnemyState.WalkToEnemy);
    }
    private void EnemyAttack()
    {
        animStateMgr.ChangeState(EnemyState.Attack);
    }
    private void EnemyWalk()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            wayPoint.CenterPos, curSpeed* Time.deltaTime);
        animStateMgr.ChangeState(EnemyState.Walk);
    }
    private void EnemyIdle()
    {
        animStateMgr.ChangeState(EnemyState.Idle);
    }


    private void OnAttackEnd()
    {
        if(attackTarget!=null)
            attackTarget.GetComponent<ITakeDamage>().TakeDamage(atk);
    }
    
}
