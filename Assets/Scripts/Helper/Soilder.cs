using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Follower;
using Player;
using UnityEngine;


public class Soilder : MonoBehaviour
{
    public float atkDis;
    public float speed;
    public int atk;
    
    private Transform _house;
    private GameObject _attackTarget;
    private Vector2 _followPoint;
    private Vector2 _lastPoint;
    private BaseEnemyAnimStateMgr animStateMgr;
    private BoxCollider2D box;
    private int curAtk;
    
    private float curSpeed;
    private float _disToPlayer;
    private MoveState _moveType;
    
    private void Start()
    {
        animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();
        box = GetComponent<BoxCollider2D>();
        
        curAtk = atk;
        curSpeed = speed;
    }
    
    
    private void Update()
    {
        if (animStateMgr.CurState() == EnemyState.Walk && _moveType != MoveState.CanAttack)
        {
            Move();
        }
        CheckAround();
    }

    void Move()
    {
        if (_moveType != MoveState.FollowEnemy && (Vector2) transform.position != _followPoint)
        {
            float disSpeed = _disToPlayer * curSpeed;    
            transform.position = Vector3.MoveTowards(transform.position, _followPoint, disSpeed * Time.deltaTime);
        }
        else if(_moveType == MoveState.FollowEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, _attackTarget.transform.position, (curSpeed + 3) * Time.deltaTime);
        }
      
    }
    
//    private void OnDrawGizmos()
//    {
//        Vector2 checkSize = new Vector2(atkDis, atkDis);
//        Vector2 checkPoint = (Vector2)transform.position;
//        Gizmos.DrawWireCube(checkPoint, checkSize);
//    }

    void FireFireBall()
    {
        if(_attackTarget == null || !_attackTarget.activeSelf)
            return;
        
        GameObject bullet = PoolMgr.GetInstance().PopObj("PeaBall");
        bullet.tag = "PlayerBullet";
        bullet.GetComponent<BaseBullet>().SetShotDir(curAtk, _attackTarget.transform.position, transform.position);
         
    }
    
    void CheckAround()
    {
        Vector2 checkSize = new Vector2(atkDis, atkDis);
        Vector2 checkPoint = (Vector2)transform.position + box.offset;
        Collider2D attackRange = Physics2D.OverlapBox(checkPoint, checkSize, 0, LayerMask.GetMask("Enemy"));
        
        _disToPlayer = Vector2.Distance(transform.position, PlayerStateMgr.Instance.transform.position);
        
        if (attackRange != null)
        {
            if(_attackTarget == null || _attackTarget.activeSelf)
                _attackTarget = attackRange.gameObject;
            
            TryToChangeType(MoveState.CanAttack);
        }
        else
        {
            if (_attackTarget != null)
            {
                TryToChangeType(MoveState.FollowEnemy);
                if (Vector2.Distance(transform.position, _attackTarget.transform.position) > atkDis)
                {
                    _attackTarget = null;
                    animStateMgr.TryChangeState(EnemyState.Idle);
                }
            }
        }
        
        if (_disToPlayer < 1)
        {
            TryToChangeType(MoveState.InRange);
        }
        else
        {
            _followPoint = PlayerStateMgr.Instance.FollowPoint;
            TryToChangeType(MoveState.OutRange);
        }
    }

    void TryToChangeType(MoveState moveState)
    {
        if(moveState == _moveType)
            return;
        switch (moveState)
        {
            case MoveState.CanAttack:
                if (_moveType == MoveState.InRange || _moveType == MoveState.FollowEnemy)
                {
                    _moveType = MoveState.CanAttack;
                    animStateMgr.TryChangeState(EnemyState.Attack);
                }
                break;
            case MoveState.InRange:
                if (_moveType == MoveState.OutRange)
                {
                    _moveType = MoveState.InRange;
                    animStateMgr.TryChangeState(EnemyState.Idle);
                }
                break;
            case MoveState.OutRange:
                _moveType = MoveState.OutRange;
                animStateMgr.ChangeState(EnemyState.Walk);
                break;
            case MoveState.FollowEnemy:
                if (_moveType == MoveState.CanAttack && _attackTarget!= null)
                    if (Vector2.Distance(transform.position, _attackTarget.transform.position) > atkDis - 2)
                {
                    _moveType = MoveState.FollowEnemy;
                    animStateMgr.TryChangeState(EnemyState.Walk);
                }
                break;
        }
        
    }

    private void Dead()
    {
        if (_house != null)
        {
            transform.parent = _house;
            transform.localPosition = Vector3.zero;
        }
        gameObject.SetActive(false);

    }

    public void SetHouse(Transform house)
    {
        _house = house;
    }
}
