using System;
using System.Collections;
using System.Collections.Generic;
using GameUi;
using UnityEngine;

public class BaseProperty : MonoBehaviour
{
    private CharacterUiMgr uimgr;
    
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
        uimgr = transform.Find("Canvas").GetComponent<CharacterUiMgr>();
        _curHp = maxHp;
        curSpeed = moveSpeed;
    }
    

    protected virtual void Dead()
    {
        Destroy(gameObject);
    }

    protected void HpBarUpDate()
    {
        uimgr.HpBarUpdate(_curHp,maxHp);
    }
}
