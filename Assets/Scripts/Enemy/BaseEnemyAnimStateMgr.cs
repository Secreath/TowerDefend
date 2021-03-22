using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class BaseEnemyAnimStateMgr : MonoBehaviour
    {
        private EnemyState _enemyState;
        private EnemyState _lastState;
        private Animator _anim;
        private BaseEnemy _enemy;
        

        void Start()
        {
            _anim = GetComponent<Animator>();
            _enemy = GetComponent<BaseEnemy>();
            _anim.SetBool("isWalk", true);
        }

        public virtual void OnDeadEnd()
        {
            Destroy(gameObject);
        }

        public virtual void OnAttackEnd()
        {
            _enemy.CanMove(true);
        }
        
        public virtual void TryChangeState(EnemyState enemyState)
        {
            if(enemyState == _enemyState)
                return;
                
            switch (enemyState)
            {
                case EnemyState.Idle:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Walk:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Attack:
                    _enemyState = enemyState;
                    break;
            }
                
            OnEnemyStateChange(_enemyState);
        }
        
        public void ChangeState(EnemyState enemyState)
        {
            if(enemyState == _enemyState)
                return;
                
            switch (enemyState)
            {
                case EnemyState.Idle:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Walk:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Attack:
                    _enemy.CanMove(false);
                    _enemyState = enemyState;
                    break;
                case EnemyState.WalkToEnemy:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Dead:
                    _enemyState = enemyState;
                    break;
            }
                
            OnEnemyStateChange(_enemyState);
        }
            
        protected void OnEnemyStateChange(EnemyState enemyState)
        {
            if(enemyState == _lastState)
                return;
            _lastState = enemyState;
                
            StateResume();
                
            switch (enemyState)
            {
                case EnemyState.Walk:
                case EnemyState.WalkToEnemy:
                    _anim.SetBool("isWalk", true);
                    break;
                case EnemyState.Attack:
                    _anim.SetBool("isAttack", true);
                    break;
                case EnemyState.Dead:
                    _anim.SetBool("isDie", true);
                    break;
            }
        }
        //重置所有状态
        void StateResume()
        {
            _anim.SetBool("isWalk", false );
            _anim.SetBool("isAttack", false);
            _anim.SetBool("isDie", false);
        }

        public EnemyState CurState()
        {
            return _enemyState;
        }
        
    }
    

}
