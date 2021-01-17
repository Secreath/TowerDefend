using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Witch : BaseEnemy
{
    public GameObject fireBall;
    private void Update()
    {
        if (enemyState == EnemyState.Walk)
        {
            Move();
        }else if (enemyState == EnemyState.Attack)
        {
            Attack();
        }
        CheckAround();
    }

    void Move()
    {
        animStateMgr.TryChangeState(EnemyState.Walk);
        transform.Translate(Vector2.right * _curMoveSpeed* Time.deltaTime);
//        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    void Attack()
    {
        animStateMgr.TryChangeState(EnemyState.Attack);
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
        GameObject fireBall = PoolMgr.GetInstance().PopObj("FireBall");
        fireBall.GetComponent<BaseBullet>().Damage = atk;
        fireBall.tag = "EnemyBullet";
        fireBall.transform.position = transform.position;
        fireBall.GetComponent<Rigidbody2D>().velocity = Vector2.right; 
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

        Debug.Log(collider != null);
        if (collider != null)
        {
            enemyState = EnemyState.Attack;
        }
        else
        {
            enemyState = EnemyState.Walk;
        }
    }
}
