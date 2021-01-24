using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Witch : BaseEnemy
{
    public GameObject fireBall;
    
    private GameObject _attackTarget;
    
    private void Update()
    {
        if (animStateMgr.CurState() == EnemyState.Walk)
        {
            Move();
        }
        CheckAround();
    }

    void Move()
    {
        transform.Translate(Vector2.right * _curMoveSpeed* Time.deltaTime);
//        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }


//    private void OnDrawGizmos()
//    {
//        Vector2 checkSize;
//        if(box.bounds.size.x > box.bounds.size.y)
//            checkSize = new Vector2(box.bounds.size.x,box.bounds.size.x) * 2;
//        else
//            checkSize = new Vector2(box.bounds.size.y,box.bounds.size.y) * 2;
//        Vector2 checkPoint = (Vector2)transform.position + box.offset;
//        Gizmos.DrawWireCube(checkPoint, checkSize);
//    }

    void FireFireBall()
    {
        if(_attackTarget == null || !_attackTarget.activeSelf)
            return;
        GameObject bullet = PoolMgr.GetInstance().PopObj("FireBall");
        bullet.tag = "EnemyBullet";
        bullet.GetComponent<BaseBullet>().SetShootDir(atk, _attackTarget.transform.position, transform.position);
    }
    
    void CheckAround()
    {
        Vector2 checkSize;
        if(box.bounds.size.x > box.bounds.size.y)
            checkSize = new Vector2(box.bounds.size.x,box.bounds.size.x) * 2;
        else
            checkSize = new Vector2(box.bounds.size.y,box.bounds.size.y) * 2;
        
        Vector2 checkPoint = (Vector2)transform.position + box.offset;
        Collider2D collider = Physics2D.OverlapBox(checkPoint, checkSize, 0, LayerMask.GetMask("Block"));
        
        if (collider != null)
        {
            if(_attackTarget == null || _attackTarget.activeSelf)
                _attackTarget = collider.gameObject;
            animStateMgr.TryChangeState(EnemyState.Attack);
        }
        else
        {
            animStateMgr.TryChangeState(EnemyState.Walk);
        }
    }
}
