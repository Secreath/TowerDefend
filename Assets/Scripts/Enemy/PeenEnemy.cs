using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class PeenEnemy : BaseEnemy
    {
        public float attackRadius;
        public float checkRadius;

        private Vector2 checkPoint;
        

        protected override void Start()
        {
            base.Start();
            if(box.bounds.size.x > box.bounds.size.y)
                checkRadius = box.bounds.size.x * 2;
            else
                checkRadius = box.bounds.size.y * 2;
        }
     
        private void FixedUpdate()
        {
            if(!start)
                return;
            CheckAround();
            FaceToTarget();
        }

        void CheckAround()
        {
            checkPoint = (Vector2)transform.position + box.offset;
            Collider2D enemyCircle = Physics2D.OverlapCircle(checkPoint, checkRadius,LayerMask.GetMask("Block"));
            
            if (enemyCircle != null)
            {
                target = GameManager.GetPointByPos(enemyCircle.transform.position);
                AttackModel(enemyCircle.transform);
            }
            else
            {
                target = wayPoint;
                if(animStateMgr.CurState() != EnemyState.Walk)
                    StartCoroutine(MoveToEndPoint());
            }
        }

        private void MoveToPoint()
        {
            if (Vector2.Distance(transform.position, wayPoint.CenterPos) > 0.1f)
            {
                Vector2 dir = (wayPoint.CenterPos - transform.position).normalized;
                transform.Translate(dir * curSpeed * Time.deltaTime,Space.World);
            }
            else
            {
                if (pathQueue.Count < 1)
                {
                    
                    Arrived();
                }
                else
                {
                    wayPoint = pathQueue.Dequeue();
                }
            }
        }
        private IEnumerator MoveToEndPoint()
        {
            
            animStateMgr.ChangeState(EnemyState.Walk);
            while (animStateMgr.CurState() == EnemyState.Walk)
            {
                if (Vector2.Distance(transform.position, wayPoint.CenterPos) > 0.1f)
                {
                    Move(wayPoint.CenterPos);
//                    Vector2 dir = (wayPoint.position - transform.position).normalized;
//                    transform.Translate(dir * curSpeed * Time.deltaTime,Space.World);
                }
                else
                {
                    if (pathQueue.Count < 1)
                    {
                        Arrived();
                    }
                    else
                    {
                        wayPoint = pathQueue.Dequeue();
                    }
                }
                
                yield return new WaitForSeconds(0.02f);
            }

        }

        private void AttackModel(Transform enemy)
        {
            if (animStateMgr.CurState() == EnemyState.Walk)
            {
                Debug.Log("try wayPoint");
                if (Vector2.Distance(wayPoint.CenterPos, enemy.position) <= checkRadius)
                {
                    Debug.Log("UPDATE wayPoint");
                    wayPoint = pathQueue.Dequeue();
                }
            }
            
            if(Vector2.Distance(transform.position,enemy.position) > attackRadius)
            {
                animStateMgr.ChangeState(EnemyState.WalkToEnemy);
                Move(enemy.position);
//                Vector2 dir = (enemy.position - transform.position).normalized;
//                transform.Translate(dir * curSpeed * Time.deltaTime,Space.World);
//                transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, _curMoveSpeed * Time.deltaTime);
            }
            else
            {
                animStateMgr.ChangeState(EnemyState.Attack);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Block") && animStateMgr.CurState() == EnemyState.Attack)
            {
                Debug.Log("hit");
//                other.GetComponent<ITakeDamage>().TakeDamage(atk);
            }
        }

        private void Move(Vector3 target)
        {
            Vector2 dir = (target - transform.position).normalized;
            transform.Translate(dir * curSpeed * Time.deltaTime,Space.World);
        }
        
    }
    
    

}
