using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Follower;
using Player;
using UnityEngine;

namespace solider
{

    public class FollowSoldier : Soldier
    {

        public float disWithPlayer;

        protected override void CheckMoveState()
        {
            switch (_moveType)
            {
                case MoveState.FollowEnemy:
                    FollowEnemy();
                    break;
                case MoveState.FollowPlayer:
                    FollowPlayer();
                    break;
                case MoveState.FarPlayer:
                    FarPlayer();
                    break;
                case MoveState.FarOther:
                    FarOther();
                    break;
                case MoveState.Stop:
                    break;
            }
        }

        //ChangeMoveState
        protected override void ChangeMoveState()
        {
            //敌人不为空 且距离玩家距离不超过最大攻击距离
            if (_enemy != null && (Vector2.Distance(transform.position, _player.position) < _curProperty.followEnemyDis))
            {
                if (Vector2.Distance(transform.position, _enemy.position) > _curProperty.atkDis)
                {
                    _moveType = MoveState.FollowEnemy;
                }
                else
                {
                    _moveType = MoveState.Stop;
                }
            }
            else if (Vector2.Distance(transform.position, _player.position) > disWithPlayer)
            {
                _moveType = MoveState.FollowPlayer;
            }
            else if (Vector2.Distance(transform.position, _player.position) < disWithPlayer - 0.3f)
            {
                _moveType = MoveState.FarPlayer;
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

        protected override void CheckAround()
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

        public void FollowPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position,
                _player.position, _curProperty.speed * Time.deltaTime);
        }

        public void FarPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position,
                _player.position, -_curProperty.speed * Time.deltaTime);
        }

    }
}
