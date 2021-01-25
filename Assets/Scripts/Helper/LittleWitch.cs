using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Follower;
using Player;
using UnityEngine;


public class LittleWitch : BaseEnemy
{
    public float atkDis;

    private Transform _house;
    private GameObject _attackTarget;
    private Vector2 _followPoint;
    private Vector2 _lastPoint;

    private float _disToPlayer;
    private FollowState _followType;
    private void Update()
    {
        if (animStateMgr.CurState() == EnemyState.Walk && _followType != FollowState.CanAttack)
        {
            Move();
        }
        CheckAround();
    }

    void Move()
    {
        if (_followType != FollowState.FollowEnemy && (Vector2) transform.position != _followPoint)
        {
            float disSpeed = _disToPlayer * _curMoveSpeed;    
            transform.position = Vector3.MoveTowards(transform.position, _followPoint, disSpeed * Time.deltaTime);
        }
        else if(_followType == FollowState.FollowEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, _attackTarget.transform.position, (_curMoveSpeed + 3) * Time.deltaTime);
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
        bullet.GetComponent<BaseBullet>().SetShootDir(atk, _attackTarget.transform.position, transform.position);
         
    }
    
    void CheckAround()
    {
        Vector2 checkSize = new Vector2(atkDis, atkDis);
        Vector2 checkPoint = (Vector2)transform.position + box.offset;
        Collider2D attackRange = Physics2D.OverlapBox(checkPoint, checkSize, 0, LayerMask.GetMask("Enemy"));
        
        _disToPlayer = Vector2.Distance(transform.position, PlayerStateMgr.GetInstance().transform.position);
        
        if (attackRange != null)
        {
            if(_attackTarget == null || _attackTarget.activeSelf)
                _attackTarget = attackRange.gameObject;
            
            TryToChangeType(FollowState.CanAttack);
        }
        else
        {
            if (_attackTarget != null)
            {
                TryToChangeType(FollowState.FollowEnemy);
                if (Vector2.Distance(transform.position, _attackTarget.transform.position) > atkDis)
                {
                    _attackTarget = null;
                    animStateMgr.TryChangeState(EnemyState.Idle);
                }
            }
        }
        
        if (_disToPlayer < 1)
        {
            TryToChangeType(FollowState.InRange);
        }
        else
        {
            _followPoint = PlayerStateMgr.GetInstance().FollowPoint;
            TryToChangeType(FollowState.OutRange);
        }
    }

    void TryToChangeType(FollowState followState)
    {
        if(followState == _followType)
            return;
        switch (followState)
        {
            case FollowState.CanAttack:
                if (_followType == FollowState.InRange || _followType == FollowState.FollowEnemy)
                {
                    _followType = FollowState.CanAttack;
                    animStateMgr.TryChangeState(EnemyState.Attack);
                }
                break;
            case FollowState.InRange:
                if (_followType == FollowState.OutRange)
                {
                    _followType = FollowState.InRange;
                    animStateMgr.TryChangeState(EnemyState.Idle);
                }
                break;
            case FollowState.OutRange:
                _followType = FollowState.OutRange;
                animStateMgr.ChangeState(EnemyState.Walk);
                break;
            case FollowState.FollowEnemy:
                if (_followType == FollowState.CanAttack && _attackTarget!= null)
                    if (Vector2.Distance(transform.position, _attackTarget.transform.position) > atkDis - 2)
                {
                    _followType = FollowState.FollowEnemy;
                    animStateMgr.TryChangeState(EnemyState.Walk);
                }
                break;
        }
        
    }

    protected override void Dead()
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
