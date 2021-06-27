using System;
using System.Collections;
using System.Collections.Generic;
using GameUi;
using ui;
using UnityEngine;

public class BaseProperty : MonoBehaviour
{
    public Vector3 uiOffSet;
    protected CharacterUiMgr uimgr;
    
    [SerializeField]protected int atk;
    [SerializeField]protected int maxHp;
    [SerializeField]protected float moveSpeed;

    protected bool canMove;
    protected int _curHp;

    private float _curSpeed;
    
    protected float curSpeed
    {
        get
        {
            if (canMove)
                return _curSpeed;
            
            return 0;
        }
        set
        {
            if (value < 0)
                _curSpeed = 0;
            else
                _curSpeed = value;
        }
    }
    
    protected virtual void Start()
    {
        canMove = true;
        uimgr = UiManager.EnemyUi.PopCharUiBar();
        _curHp = maxHp;
        curSpeed = moveSpeed;
    }
    

    protected virtual void Dead()
    {
        UiManager.EnemyUi.PushCharUiBar(uimgr);
        Destroy(gameObject);
    }

    protected void HpBarUpDate()
    {
        uimgr.HpBarUpdate(_curHp,maxHp);
    }
    
    protected void SetUiPos(Vector3 pos)
    {
        uimgr.SetPos(pos + uiOffSet);
    }
}
