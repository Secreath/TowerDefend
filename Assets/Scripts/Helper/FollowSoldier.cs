using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Follower;
using Player;
using UnityEngine;


public class FollowSoldier : MonoBehaviour
{
    public float atkDis;
    public float maxAtkDis;
    public float speed;
    public int atk;
    public float disWithPlayer;
    public float disWithOther;
    public float disWithEnemy;
    
    private SoldierTower _house;
    private Transform _player;
    private BaseEnemyAnimStateMgr _animStateMgr;
    private BoxCollider2D _box;
    
    private Transform _otherSoldier;
    private Transform _enemy;
    
    private int _curAtk;
    private float _curSpeed;
    
    private MoveState _moveType;
    private MoveState _lastMoveType;
    private void Start()
    {
        _player = PlayerInput.Instance.transform;
        _animStateMgr = GetComponent<BaseEnemyAnimStateMgr>();
        _box = GetComponent<BoxCollider2D>();
        
        _curAtk = atk;
        _curSpeed = speed;
    }
    
    public void SetHouse(SoldierTower house)
    {
        _house = house;
        atk = house.soliderAtk;
        EventCenter.GetInstance().AddEventListener($"{house.name}UpGrade",TowerUpgrade);
        EventCenter.GetInstance().AddEventListener($"{house.name}Destory",TowerDestory);
    } 
    
    private void Update()
    {
        CheckAround();
        ChangeMoveState();
        CheckMoveState();
    }

    private void CheckMoveState()
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
    public void ChangeMoveState()
    {
        //敌人不为空 且距离玩家距离不超过最大攻击距离
        if (_enemy != null && (Vector2.Distance(transform.position, _player.position) < maxAtkDis))
        {
            if (Vector2.Distance(transform.position, _enemy.position) > atkDis)
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

    private void CheckAround()
    {
        Vector2 checkPoint = (Vector2)transform.position + _box.offset;
        _box.enabled = false;
        Collider2D other = Physics2D.OverlapCircle(checkPoint, disWithOther, LayerMask.GetMask("Soldier"));
        Collider2D enemy = Physics2D.OverlapCircle(checkPoint, disWithEnemy, LayerMask.GetMask("Enemy"));
        _box.enabled = true;
        
        if(other == null)
            _otherSoldier = null;
        else
            _otherSoldier = other.transform;
        
        if(enemy == null)
            _enemy = null;
        else
            _enemy = enemy.transform;
        
    }
    public void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            _player.position, _curSpeed * Time.deltaTime);
    }
   
    public void FollowEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            _enemy.position, _curSpeed * Time.deltaTime);
    }
    
    public void FarPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            _player.position, -_curSpeed * Time.deltaTime);
    }
    
    public void FarOther()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            _otherSoldier.transform.position, -speed/3 * Time.deltaTime);
    }

    private void TowerUpgrade()
    {
        Debug.Log("SoliderUpgrade");
    }
    
    private void TowerDestory()
    {
        Debug.Log("SoliderDestory");
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener($"{_house.name}UpGrade",TowerUpgrade);
        EventCenter.GetInstance().RemoveEventListener($"{_house.name}Destory",TowerDestory);
    }
}
