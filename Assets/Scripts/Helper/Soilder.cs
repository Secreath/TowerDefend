using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Follower;
using Player;
using UnityEngine;

namespace solider
{

    public class Soldier : MonoBehaviour
    {

        protected SoldierTower _house;
        protected Transform _player;
        protected BaseEnemyAnimStateMgr _animStateMgr;
        protected BoxCollider2D _box;

        protected Transform _otherSoldier;
        protected Transform _enemy;

        public EnemyProperty property;
        protected EnemyProperty _curProperty;

        protected MoveState _moveType;
        protected MoveState _lastMoveType;
        protected string eventId;

        
        [Serializable]
        public struct EnemyProperty
        {
            public float speed;
            public int atk;
            public int hp;
            
            public float atkDis;
            public float followEnemyDis;
            public float disWithOther;
            public float disWithEnemy;
        }

        protected virtual void Start()
        {
            _player = PlayerInput.Instance.transform;
            _animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();
            _box = GetComponent<BoxCollider2D>();

            _curProperty = property;
            
        }

        public virtual void SetHouse(SoldierTower house)
        {
            _house = house;
            _curProperty.atk = house.soliderAtk;
            eventId = house.name;
            EventCenter.GetInstance().AddEventListener($"{eventId}UpGrade", TowerUpgrade);
            EventCenter.GetInstance().AddEventListener($"{eventId}Destory", TowerDestory);
        }

        protected virtual void Update()
        {
            CheckAround();
            ChangeMoveState();
            CheckMoveState();
        }

        protected virtual void CheckMoveState()
        {
            switch (_moveType)
            {
                case MoveState.FollowEnemy:
                    FollowEnemy();
                    break;
                case MoveState.FarOther:
                    FarOther();
                    break;
                case MoveState.Stop:
                    break;
            }
        }

        //ChangeMoveState
        protected virtual void ChangeMoveState()
        {
            //敌人不为空 且距离玩家距离不超过最大攻击距离
            if (_enemy != null && !CompareTool.DisLongerThan(transform.position, _player.position,_curProperty.followEnemyDis))
            {
                
                if (CompareTool.DisLongerThan(transform,_enemy,_curProperty.atkDis))
                {
                    _moveType = MoveState.FollowEnemy;
                }
                else
                {
                    _moveType = MoveState.Stop;
                }
            }
            else if (_otherSoldier != null)
            {
                _moveType = MoveState.FarOther;
            }
            else
            {
                _moveType = MoveState.Stop;
            }
        }

        protected virtual void CheckAround()
        {
            Vector2 checkPoint = (Vector2) transform.position + _box.offset;
            _box.enabled = false;
            Collider2D other = Physics2D.OverlapCircle(checkPoint, _curProperty.disWithOther, LayerMask.GetMask("Soldier"));
            Collider2D enemy = Physics2D.OverlapCircle(checkPoint, _curProperty.disWithEnemy, LayerMask.GetMask("Enemy"));
            _box.enabled = true;

            if (other == null)
                _otherSoldier = null;
            else
                _otherSoldier = other.transform;

            if (enemy == null)
                _enemy = null;
            else
                _enemy = enemy.transform;

        }

        protected virtual void FollowEnemy()
        {
            transform.position = Vector2.MoveTowards(transform.position,
                _enemy.position, _curProperty.speed * Time.deltaTime);
        }

        protected virtual void FarOther()
        {
            transform.position = Vector2.MoveTowards(transform.position,
                _otherSoldier.transform.position, -_curProperty.speed / 3 * Time.deltaTime);
        }

        protected virtual void TowerUpgrade()
        {
            Debug.Log("SoliderUpgrade");
        }

        protected virtual void TowerDestory()
        {
            Debug.Log("SoliderDestory");
        }

        protected virtual void OnDestroy()
        {
            EventCenter.GetInstance().RemoveEventListener($"{eventId}UpGrade", TowerUpgrade);
            EventCenter.GetInstance().RemoveEventListener($"{eventId}Destory", TowerDestory);
        }
    }
}